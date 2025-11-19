using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private PlayerController _player;
    public PlayerRunState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        //_player.Animator.SetBool("IsRunnig", true);
        _player.Animator.runtimeAnimatorController = _player.runController;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (_player.MoveInput.x == 0)
        {
            _player.SetState(new PlayerIdleState(_player));
        }
    }
}
