using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Bow : MonoBehaviour
{
    [Header("-----参照-----")]
    [SerializeField] private InputAction _pointAction;
    [SerializeField] private InputAction _pressAction;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _arrowTr;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _maxPullDistance = 3f;
    [SerializeField] private float _maxPower = 20f;
    [SerializeField] private Vector3 _minArrowSize = Vector3.zero;
    [SerializeField] private Vector3 _maxArrowSize = Vector3.one * 1.5f;
    [SerializeField] private float _grabTime = 1f;
    [SerializeField] private float _angleOffset = 90f;
    [SerializeField, Range(0f, 90f)] private float _maxAngleFromUp = 90f;

    private Transform _tr;
    private float _timer;
    private bool _isDragging = false;
    private Vector2 _pressPos;
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
        _timer = 0;

        _arrowTr.gameObject.SetActive(true);
        ChangeArrowSize(0f);
    }
    private void OnDisable()
    {
        _arrowTr.gameObject.SetActive(false);
    }
    private void OnPressStart(InputAction.CallbackContext ctx)
    {
        _pressPos = _camera.ScreenToWorldPoint(_pointAction.ReadValue<Vector2>());

        _isDragging = true;
        enabled = true;
    }

    private void OnPressEnd(InputAction.CallbackContext ctx)
    {
        if (!_isDragging) return;

        _isDragging = false;
        enabled = false;
        if (_timer <= _grabTime) return;
        Vector2 pull = GetPullVector();
        Shoot(pull);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer <= _grabTime) return;

        Vector2 pullVector = GetPullVector();
        float angle = Mathf.Atan2(-pullVector.y, -pullVector.x) * Mathf.Rad2Deg + _angleOffset;
        _tr.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private Vector2 GetPullVector()
    {
        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(_pointAction.ReadValue<Vector2>());
        Vector2 pullVector = _pressPos - mouseWorldPos;
        pullVector = Vector2.ClampMagnitude(pullVector, _maxPullDistance);

        ChangeArrowSize(pullVector.magnitude / _maxPullDistance);
        if (pullVector.sqrMagnitude < 0.0001f) return pullVector;

        Vector2 shootDir = pullVector.normalized;
        float angleFromUp = Vector2.SignedAngle(Vector2.up, shootDir);
        float clampedAngle = Mathf.Clamp(angleFromUp, -_maxAngleFromUp, _maxAngleFromUp);

        Vector2 clampedShootDir = Quaternion.Euler(0f, 0f, clampedAngle) * Vector2.up;
        return clampedShootDir * pullVector.magnitude;
    }
    private void Shoot(Vector2 pullVector)
    {
        float powerRatio = pullVector.magnitude / _maxPullDistance;
        Vector2 direction = pullVector.normalized;

        GameObject arrow = Instantiate(_arrowPrefab, _firePoint.position, _tr.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * (powerRatio * _maxPower);
    }

    private void ChangeArrowSize(float percentage)
    {
        _arrowTr.localScale = Vector3.Lerp(_minArrowSize,_maxArrowSize,percentage);
    }
}
