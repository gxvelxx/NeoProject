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

    private SpriteRenderer _sprite;

    private IPlayerState _currentState;

    private Animator _animator;

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

        _sprite = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();
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
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;

        _leftAttackAction.performed -= OnLeftAttack;
    }

    private void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        if (!(_currentState is PlayerLeftAttack))
        {
            //시선처리
            if (_moveInput.x > 0) _sprite.flipX = false;
            else if (_moveInput.x < 0) _sprite.flipX = true;

            //이동
            Vector2 velocity = _rigid.linearVelocity;
            velocity.x = _moveInput.x * _moveSpeed;
            _rigid.linearVelocity = velocity;
        }
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
        SetState(new PlayerLeftAttack(this));
    }

    //상태처리
    public void SetState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }    
}
