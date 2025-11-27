using UnityEngine;

public class EnemyHitState : IEnemyState
{
    private EnemyController _enemy;
    private float _hitDuration = 0.2f; // 경직시간
    private float _timer = 0f;
    public EnemyHitState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _timer = 0;

        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero; // 이동중단

        _enemy.Animator.SetTrigger("Hit");
    }

    public void Exit()
    {
        
    }    

    public void Update()
    {
        _enemy.LookPlayer();

        AnimatorStateInfo state = _enemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Hit") && state.normalizedTime < 1f)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _hitDuration)
        {
            _enemy.SetState(new EnemyChaseState(_enemy));
        }
    }

    public void FixedUpdate()
    {
        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;
    }
}
