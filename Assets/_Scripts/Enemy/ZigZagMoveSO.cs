using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMove/ZigZag")]
public class ZigZagMoveSO : EnemyMoveSO
{
    [SerializeField] private float sideSpeed = 2f;
    [SerializeField] private float changeInterval = 0.5f;

    public override void Initialize(Enemy enemy)
    {
        enemy.MoveTime = 0;
    }

    public override void Move(Enemy enemy)
    {
        enemy.MoveTime += Time.deltaTime;

        int dir = Mathf.FloorToInt(enemy.MoveTime / changeInterval) % 2 == 0 ? -1 : 1;

        Vector3 move = new Vector3(
            dir * sideSpeed,
            -enemy.Data.MoveSpeed,
            0);

        enemy.transform.position += move * Time.deltaTime;
    }
}