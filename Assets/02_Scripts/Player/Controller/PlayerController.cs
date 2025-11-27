using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rigid;
    private Vector2 _moveInput;

    [Header("Input Action")]
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _leftAttackAction;
    private InputAction _rightAttackAction;

    private SpriteRenderer _sprite;

    private IPlayerState _currentState;

    private Animator _animator;

    private AttackHitbox _attackHitbox;

    [Header("Animator Controllers")]
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController runController;
    public RuntimeAnimatorController ComboAController;
    public RuntimeAnimatorController ComboBController;
    public RuntimeAnimatorController ComboCController;
    public RuntimeAnimatorController ComboDController;

    [Header("Properties")]
    public float MoveSpeed => _moveSpeed;
    public Rigidbody2D Rigid => _rigid;
    public Vector2 MoveInput => _moveInput;
    public Animator Animator => _animator;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _leftAttackAction = _playerInput.actions["LeftAttack"];
        _rightAttackAction = _playerInput.actions["RightAttack"];

        _sprite = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

        _attackHitbox = GetComponentInChildren<AttackHitbox>();
    }

    private void Start()
    {
        SetState(new PlayerIdleState(this));
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;

        _leftAttackAction.performed += OnLeftAttack;
        _rightAttackAction.performed += OnRightAttack;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;

        _leftAttackAction.performed -= OnLeftAttack;
        _rightAttackAction.performed -= OnRightAttack;
    }

    private void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        //Axis값 (-1 ~ +1)
        float value = ctx.ReadValue<float>();
        _moveInput = new Vector2(value, 0);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        _moveInput = Vector2.zero;
    }

    //공격
    private void OnLeftAttack(InputAction.CallbackContext ctx)
    {
        //키입력 가져오기
        string key = ctx.control.displayName;
        //입력체크
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        char pressedKey = key[0]; // S,D,F

        SetState(new PlayerLeftAttackState(this, pressedKey));
    }
    private void OnRightAttack(InputAction.CallbackContext ctx)
    {
        //키입력 가져오기
        string key = ctx.control.displayName;
        //입력체크
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        char pressedKey = key[0]; // J,K,L

        SetState(new PlayerRightAttackState(this, pressedKey));
    }

    //상태처리
    public void SetState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    //애니 이벤트
    public void StartAttackHit()
    {
        if (_attackHitbox == null)
        {
            _attackHitbox = GetComponentInChildren<AttackHitbox>();
            if (_attackHitbox == null)
                return;
        }

        // 플레이어 방향 전달
        bool facingRight = transform.localScale.x > 0f;
        _attackHitbox.SetDirection(facingRight);

        _attackHitbox.EnableHitbox();
    }
    public void EndAttackHit()
    {
        if (_attackHitbox == null) return;
        _attackHitbox.DisableHitbox();
    }
}
