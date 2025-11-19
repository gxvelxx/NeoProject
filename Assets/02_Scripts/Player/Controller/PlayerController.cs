using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rigid;
    private Vector2 _moveInput;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];

        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
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

    private void FixedUpdate()
    {
        //시선처리
        if (_moveInput.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_moveInput.x < 0)
        {
            _sprite.flipX = true;
        }

        //이동
        Vector2 velocity = _rigid.linearVelocity;
        velocity.x = _moveInput.x * _moveSpeed;
        _rigid.linearVelocity = velocity;
    }
}
