using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    public event Action<Enemy> OnReturn;
    public int Score => _enemyData.Score;
    public EnemyStatusSO Data => _enemyData;
    public int EnemyID { get; private set; }

    [Header("エネミーデータ")]
    [SerializeField] private EnemyStatusSO _enemyData;

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
        _enemyData.MoveStrategy.Move(this);
    }

    public void HitArrow()
    {
        Return();
    }

    private void Return()
    {
        OnReturn?.Invoke(this);
    }
}