using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemy;
    private float _attackCooldown = 1f; //공격속도
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
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        //공격범위를 벗어나면
        if (_enemy.Player != null)
        {
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.Player.position);
            if (distance > _enemy.AttackRange)
            {
                _enemy.SetState(new EnemyChaseState(_enemy));
                return;
            }
        }

        _timer += Time.deltaTime;

        if (_timer >= _attackCooldown)
        {
            _timer = 0f;

            DoAttack();
        }
    }

    public void FixedUpdate()
    {
        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;
    }

    private void DoAttack()
    {
        Debug.Log("플레이어 공격성공");
    }
}
