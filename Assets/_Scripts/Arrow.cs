using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [Header("-----参照-----")]
    [SerializeField] private Transform _tr;
    [SerializeField] private Rigidbody2D _rb;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private float _angleOffset = 90f;

    private bool _isStuck = false;

    private bool _beforePause = false;
    private Vector2 _savedVelocity;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
    }
    private void Update()
    {
        if(GamePauseManager.IsPaused != _beforePause)
        {
            if(GamePauseManager.IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
            _beforePause = GamePauseManager.IsPaused;
        }

        if (_isStuck || GamePauseManager.IsPaused) return;

        if(_rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            _tr.rotation = Quaternion.Euler(0f, 0f, 
                Mathf.Atan2(_rb.linearVelocity.y,_rb.linearVelocity.x)
                * Mathf.Rad2Deg + _angleOffset);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isStuck) return;

        if(other.TryGetComponent<IHitable>(out var obj))
        {
            obj.HitArrow();
            InkManager.Instance.DisplayInk(other.transform.position);
            _scoreManager.AddCombo();
        }
        else
        {
            _isStuck = true;
            Destroy(gameObject);
        }
    }

    private void Pause()
    {
        _savedVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;
    }

    private void Resume()
    {
        _rb.simulated = true;
        _rb.linearVelocity = _savedVelocity;
    }
}
