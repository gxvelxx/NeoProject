using UnityEngine;

public class PlayerLeftAttack : IPlayerState
{
    private PlayerController _player;
    private float _attackImpulse = 10f; // 순간적인힘
    public PlayerLeftAttack(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Animator.runtimeAnimatorController = _player.ComboAController;

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
}
