using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyController _enemy;
    public EnemyChaseState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {        
        _enemy.Animator.SetBool("isRunning", true);        
    }

    public void Exit()
    {
        _enemy.Animator.SetBool("isRunning", false);
    }

    public void Update()
    {
        if (_enemy.Player == null)
            return;
        
        //공격범위 안이면 공격상태로
        float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.position);
        if (distance <= _enemy.AttackRange)
        {
            Debug.Log("공격 범위 도달 → EnemyAttackState로 전환");
            _enemy.SetState(new EnemyAttackState(_enemy));
            return;
        }
    }

    public void FixedUpdate()
    {
        if (_enemy.Player == null)
            return;
        
        Vector2 direction = (_enemy.Player.position - _enemy.transform.position).normalized;

        _enemy.Rigid.linearVelocity = new Vector2(direction.x * _enemy.MoveSpeed, _enemy.Rigid.linearVelocity.y);
    }    
}
