using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Move/Straight")]
public class StraightMoveSO : EnemyMoveSO
{
    public override void Initialize(Enemy enemy)
    {
        //‰Šú‰»‚ ‚éê‡‚Í‚±‚±‚É“ü‚ê‚é—\’è
    }

    public override void Move(Enemy enemy)
    {
        enemy.transform.position += Vector3.down * enemy.Data.MoveSpeed * Time.deltaTime;
    }
}