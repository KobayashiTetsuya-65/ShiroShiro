using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    public event Action<Enemy> OnReturn;
    public int Score => _enemyData.Score;
    public EnemyStatusSO Data => _enemyData;
    public int EnemyID { get; private set; }
    public float MoveTime;

    [Header("エネミーデータ")]
    [SerializeField] private EnemyStatusSO _enemyData;
    [SerializeField] private bool _isFeverEnemy = false;
    [SerializeField] private float _castleLineY = -3f;

    public void Initialize(int enemyID)
    {
        EnemyID = enemyID;
    }

    private void Start()
    {
        _enemyData.MoveStrategy.Initialize(this);
    }

    private void Update()
    {
        if(GamePauseManager.IsPaused) return;
        _enemyData.MoveStrategy.Move(this);
        CheckCastle();
    }

    public void HitArrow()
    {
        AudioManager.Instance.PlaySE(SEType.Hit);
        Return();
    }

    private void Return()
    {
        HitEffectManager.Instance.Play(transform.position);
        ScoreManager.Instance.AddScore(Score);
        if(_isFeverEnemy)
        {
            ScoreManager.Instance.FeverTime();
        }
        OnReturn?.Invoke(this);
    }

    private void CheckCastle()
    {
        if (transform.position.y <= _castleLineY)
        {
            Castle.Instance.TakeDamage(_enemyData.CastleDamage);
            Debug.Log(Castle.Instance.CurrentHp);
            OnReturn?.Invoke(this);
        }
    }
}