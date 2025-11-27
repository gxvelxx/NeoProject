using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private PlayerController _player;
    private float _timer = 0f;
    private float _dieDuration = 1f;
    public PlayerDieState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _timer = 0f;

        if (_player.Rigid != null)
            _player.Rigid.linearVelocity = Vector2.zero;

        if (_player.DieController != null)
            _player.Animator.runtimeAnimatorController = _player.DieController;
    }

    public void Exit()
    {
        
    }   

    public void Update()
    {
        AnimatorStateInfo state = _player.Animator.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime < 1f)
            return;

        _timer += Time.deltaTime;
        
        if (_timer >= _dieDuration)
        {
            //시간되면 씬전환
        }
    }

    public void FixedUpdate()
    {
        _player.Rigid.linearVelocity = Vector2.zero;
    }
}
