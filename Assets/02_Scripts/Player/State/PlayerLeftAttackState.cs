using UnityEngine;

public class PlayerLeftAttackState : IPlayerState
{
    private PlayerController _player;
    private float _attackImpulse = 10f; // 순간적인힘

    private char _pressedKey;
    private static char _lastKey = '\0';
    private static int _comboStep = 0;
    public PlayerLeftAttackState(PlayerController player, char pressedKey)
    {
        _player = player;
        _pressedKey = pressedKey;
    }

    public void Enter()
    {
        //같은 키입력이면 콤보가 안돼야
        if (_pressedKey == _lastKey)
        {
            _comboStep = 0;
        }
        else
        {
            //A->D까지 4단
            _comboStep = _comboStep % 4;
        }

        _lastKey = _pressedKey;

        //콤보단계별 모션적용
        RuntimeAnimatorController comboAnimCtrl =
            _comboStep switch
            {
                0 => _player.ComboAController,
                1 => _player.ComboBController,
                2 => _player.ComboCController,
                3 => _player.ComboDController,
                _ => _player.ComboAController // '\0'
            };

        _player.Animator.runtimeAnimatorController = comboAnimCtrl;

        //왼쪽으로        
        Vector2 attackDirection = Vector2.left;
        _player.Animator.GetComponent<SpriteRenderer>().flipX = true;

        //순간적인힘
        _player.Rigid.linearVelocity = Vector2.zero;
        _player.Rigid.AddForce(attackDirection * _attackImpulse, ForceMode2D.Impulse);
    }

    public void Exit()
    {
        Vector2 velocity = _player.Rigid.linearVelocity;
        velocity.x = 0;
        _player.Rigid.linearVelocity = velocity;

        //풀콤보시
        _comboStep++;
        if (_comboStep > 3)
        {
            _comboStep = 0;
        }
    }

    public void Update()
    {
        //모션끝나면 아이들상태로
        AnimatorStateInfo stateInfo = _player.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            _player.SetState(new PlayerIdleState(_player));
        }
    }

    public void FixedUpdate()
    {
        
    }
}
