using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float _spawnInterval = 2f;
    public Transform[] _spawnPoints;

    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnEnemyRandomPoint();
        }
    }

    private void SpawnEnemyRandomPoint()
    {
        if (_spawnPoints.Length == 0)
            return;

        int index = Random.Range(0, _spawnPoints.Length);
        EnemyPool.Instance.SpawnEnemy(_spawnPoints[index].position);
    }
}
