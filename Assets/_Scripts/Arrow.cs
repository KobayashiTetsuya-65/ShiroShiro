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

    private void Update()
    {
        if (_isStuck) return;

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
        }

        _isStuck = true;
    }
}
