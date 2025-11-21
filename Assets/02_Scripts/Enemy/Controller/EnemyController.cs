using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Setting")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _attackRagne = 1f;

    private Rigidbody2D _rigid;

    private IEnemyState _currentState;

    private Transform _player;

    [Header("Properties")]
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRagne;
    public Rigidbody2D Rigid => _rigid;
    public Transform Player => _player;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            _player = playerObject.transform;

        SetState(new EnemyChaseState(this));
    }

    
    void Update()
    {
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void SetState(IEnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
