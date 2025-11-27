using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    public GameObject _enemyPrefab;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var obj = Instantiate(_enemyPrefab);
                obj.transform.SetParent(this.transform); // 풀객체 하위로 정리
                obj.SetActive(false);
                obj.GetComponent<EnemyController>().SetPool(_pool); // 컨트롤러한테 풀 객체 전달
                return obj;
            },
            actionOnGet: obj =>                           // Get을 수행할거
            {
                obj.transform.SetParent(this.transform);  // 풀에서 꺼낼 때도 정리
                obj.SetActive(true);
            },
            actionOnRelease: obj => obj.SetActive(false),   
            actionOnDestroy: obj => Destroy(obj)            // 파괴할거
        );
    }

    public GameObject SpawnEnemy(Vector3 position)
    {
        GameObject enemy = _pool.Get();
        enemy.transform.position = position;
        return enemy;
    }
}
