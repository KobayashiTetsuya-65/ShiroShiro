using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMove/Sine")]
public class SineMoveSO : EnemyMoveSO
{
    [SerializeField] private float amplitude = 1.5f;
    [SerializeField] private float frequency = 3f;

    public override void Initialize(Enemy enemy)
    {
        enemy.MoveTime = 0;
    }

    public override void Move(Enemy enemy)
    {
        enemy.MoveTime += Time.deltaTime;

        Vector3 pos = enemy.transform.position;

        pos.y -= enemy.Data.MoveSpeed * Time.deltaTime;
        pos.x += Mathf.Sin(enemy.MoveTime * frequency)
                 * amplitude
                 * Time.deltaTime;

        enemy.transform.position = pos;
    }
}