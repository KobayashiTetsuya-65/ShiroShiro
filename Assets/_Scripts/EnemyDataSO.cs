using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int Score;
    public float MoveSpeed = 3f;
    public EnemyMoveSO MoveStrategy;
}
