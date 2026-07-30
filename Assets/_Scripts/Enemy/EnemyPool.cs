using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyDataBase _enemyDataBase;

    private readonly Dictionary<int, Queue<Enemy>> _pool = new();

    public Enemy Get(int enemyID)
    {
        if (!_pool.TryGetValue(enemyID, out Queue<Enemy> queue))
        {
            queue = new Queue<Enemy>();
            _pool.Add(enemyID, queue);
        }

        if (queue.Count > 0)
        {
            Enemy enemy = queue.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        Enemy prefab = _enemyDataBase.GetEnemyData(enemyID).EnemyObject;

        Debug.Log(prefab.GetType());

        Enemy newEnemy = Instantiate(prefab).GetComponent<Enemy>();

        newEnemy.OnReturn += Return;

        return newEnemy;
    }

    public void Return(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        if (!_pool.TryGetValue(enemy.EnemyID, out Queue<Enemy> queue))
        {
            queue = new Queue<Enemy>();
            _pool.Add(enemy.EnemyID, queue);
        }

        queue.Enqueue(enemy);
    }
}