using System.Collections;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemy;
    private float _attackCooldown = 0.5f; //공격속도
    private float _timer = 0f;

    public EnemyAttackState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _timer = 0f;

        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;

        _enemy.Animator.SetBool("isRunning", false);
        _enemy.Animator.SetTrigger("Attack");
    }    

    public void Exit()
    {
        
    }

    public void Update()
    {
        _enemy.LookPlayer();

        AnimatorStateInfo state = _enemy.Animator.GetCurrentAnimatorStateInfo(0);
        
        if (state.IsName("Punch") && state.normalizedTime < 1f)
            return;

        float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.position);

        if (distance > _enemy.AttackRange)
        {
            _enemy.SetState(new EnemyChaseState(_enemy));
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= _attackCooldown)
        {
            _timer = 0f;
            _enemy.Animator.SetTrigger("Attack");
        }
    }

    public void FixedUpdate()
    {
        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;
    }    
}
