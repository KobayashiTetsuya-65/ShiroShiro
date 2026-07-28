using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    public Action<Enemy> onDead;
    public int Score => _enemyData.Score;
    public EnemyDataSO Data => _enemyData;

    [Header("エネミーデータ")]
    [SerializeField] private EnemyDataSO _enemyData;

    private void Start()
    {
        _enemyData.MoveStrategy.Initialize(this);
    }

    private void Update()
    {
        _enemyData.MoveStrategy.Move(this);
    }

    public void HitArrow()
    {
        Dead();
    }

    private void Dead()
    {
        onDead?.Invoke(this);

        Destroy(gameObject);
    }
}