using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private StageDataSO _stageData;
    [SerializeField] private EnemyPool _enemyPool;

    [Header("ƒXƒ|[ƒ“”ÍˆÍ")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _spawnOffsetY = 1f;

    private float _timer;
    private float _spawnTimer;

    private void Update()
    { 
        if(GamePauseManager.IsPaused) return;

        _timer += Time.deltaTime;

        SpawnWave wave = GetCurrentWave();

        if (wave == null)
            return;

        _spawnTimer += Time.deltaTime;

        float interval = 1f / wave.SpawnPerSecond;

        while (_spawnTimer >= interval)
        {
            _spawnTimer -= interval;

            Spawn(wave);
        }
    }

    private SpawnWave GetCurrentWave()
    {
        foreach (var wave in _stageData.Waves)
        {
            if (_timer >= wave.StartTime &&
                _timer < wave.EndTime)
            {
                return wave;
            }
        }

        return null;
    }

    private void Spawn(SpawnWave wave)
    {
        int enemyID = GetRandomEnemyID(wave);

        Enemy enemy = _enemyPool.Get(enemyID);

        enemy.transform.position = GetSpawnPosition();
    }

    private int GetRandomEnemyID(SpawnWave wave)
    {
        int totalWeight = 0;

        foreach (var enemy in wave.Enemies)
            totalWeight += enemy.Weight;

        int random = Random.Range(0, totalWeight);

        foreach (var enemy in wave.Enemies)
        {
            random -= enemy.Weight;

            if (random < 0)
                return enemy.EnemyID;
        }

        return wave.Enemies[0].EnemyID;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 left = _camera.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 right = _camera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float x = Random.Range(left.x, right.x);
        float y = left.y + _spawnOffsetY;

        return new Vector3(x, y, 0);
    }
}