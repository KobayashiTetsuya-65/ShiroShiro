using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    [Header("-----参照-----")]
    [SerializeField] private InputAction _pointAction;
    [SerializeField] private InputAction _pressAction;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _firePoint;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _maxPullDistance = 3f;
    [SerializeField] private float _maxPower = 20f;
    [SerializeField] private float _grabRadius = 1f;
    [SerializeField] private float _angleOffset = 90f;
    [SerializeField, Range(0f, 90f)] private float _maxAngleFromUp = 90f;

    private Transform _tr;
    private bool _isDragging = false;
    private void Awake()
    {
        _tr = transform;
        _pressAction.started += OnPressStart;
        _pressAction.canceled += OnPressEnd;
    }
    private void Start()
    {
        enabled = false;
    }
    private void OnEnable()
    {
        _pressAction.Enable();
        _pointAction.Enable();
    }
    private void OnDisable()
    {
        
    }
    private void OnPressStart(InputAction.CallbackContext ctx)
    {
        Vector2 pressWorldPos = _camera.ScreenToWorldPoint(_pointAction.ReadValue<Vector2>());
        float distance = Vector2.Distance(_tr.position, pressWorldPos);

        if (distance > _grabRadius) return;

        _isDragging = true;
        enabled = true;
    }

    private void OnPressEnd(InputAction.CallbackContext ctx)
    {
        if (!_isDragging) return;

        Vector2 pull = GetPullVector();
        Shoot(pull);
        _isDragging = false;
        enabled = false;
    }

    void Update()
    {
        Vector2 pullVector = GetPullVector();
        float angle = Mathf.Atan2(-pullVector.y, -pullVector.x) * Mathf.Rad2Deg + _angleOffset;
        _tr.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private Vector2 GetPullVector()
    {
        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(_pointAction.ReadValue<Vector2>());
        Vector2 pullVector = (Vector2)_tr.position - mouseWorldPos;
        if (pullVector.sqrMagnitude < 0.0001f) return pullVector;

        Vector2 shootDir = pullVector.normalized;
        float angleFromUp = Vector2.SignedAngle(Vector2.up, shootDir);
        float clampedAngle = Mathf.Clamp(angleFromUp, -_maxAngleFromUp, _maxAngleFromUp);

        Vector2 clampedShootDir = Quaternion.Euler(0f, 0f, clampedAngle) * Vector2.up;
        return clampedShootDir * pullVector.magnitude; ;
    }
    private void Shoot(Vector2 pullVector)
    {
        float powerRatio = pullVector.magnitude / _maxPullDistance;
        Vector2 direction = pullVector.normalized;

        GameObject arrow = Instantiate(_arrowPrefab, _firePoint.position, _tr.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * (powerRatio * _maxPower);
    }
}
