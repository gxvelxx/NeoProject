using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerController _player;
    public PlayerIdleState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        //_player.Animator.SetBool("IsRunnig", false);
        _player.Animator.runtimeAnimatorController = _player.idleController;
    }

    public void Exit()
    {
        
    }    

    public void Update()
    {
        if (_player.MoveInput.x != 0)
        {
            _player.SetState(new PlayerRunState(_player));
        }
    }

    public void FixedUpdate()
    {
        //이동없게
        Vector2 velocity = _player.Rigid.linearVelocity;
        velocity.x = 0;
        _player.Rigid.linearVelocity = velocity;
    }
}
