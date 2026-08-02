using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyStatusSO : ScriptableObject
{
    public int Score;
    public float MoveSpeed = 3f;
    public int CastleDamage;
    public EnemyMoveSO MoveStrategy;
}
