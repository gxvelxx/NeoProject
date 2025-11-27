using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Setting")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _attackRagne = 1f;
    [SerializeField] private int _maxHp = 3;
    private int _hp;

    private bool isDead = false;

    private Rigidbody2D _rigid;

    private IEnemyState _currentState;

    private Transform _player;

    private Animator _animator;

    private ObjectPool<GameObject> _pool;    

    [Header("Properties")]
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRagne;
    public Rigidbody2D Rigid => _rigid;
    public Transform Player => _player;
    public Animator Animator => _animator;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _rigid.freezeRotation = true;
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            _player = playerObject.transform;
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

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        if (damage <= 0) return;

        Debug.Log("실제 공격 판전 발생");
        _hp -= damage;
        Debug.Log($"{gameObject.name} {damage} damage. HP: {_hp}");

        if (_hp <= 0)
        {
            isDead = true;
            SetState(new EnemyDieState(this)); // 죽는모션
            return;
        }

        SetState(new EnemyHitState(this)); // 피격모션
    }  

    public void Die()
    {
        if (_pool != null)
        {
            _pool.Release(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //적 기본세팅
    private void ResetEnemy()
    {
        isDead = false;
        _hp = _maxHp;
        _rigid.linearVelocity = Vector2.zero;

        _animator.Rebind();
        _animator.Update(0f);

        //바로 추적시작
        SetState(new EnemyChaseState(this));
    }

    public void LookPlayer()
    {
        if (_player == null)
            return;

        float direction = _player.position.x - transform.position.x;

        if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1); // 왼쪽
        else
            transform.localScale = new Vector3(1, 1, 1); // 오른쪽
    }
}
