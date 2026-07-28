using UnityEngine;

public abstract class EnemyMoveSO : ScriptableObject
{
    public abstract void Initialize(Enemy enemy);

    public abstract void Move(Enemy enemy);
}