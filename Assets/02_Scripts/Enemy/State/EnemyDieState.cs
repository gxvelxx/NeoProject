using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyController _enemy;
    private float _dieDuration = 0.8f; // 죽는시간
    private float _timer = 0f;
    public EnemyDieState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _timer = 0f;

        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;
        
        _enemy.Animator.SetTrigger("Die"); // 죽는모션        
    }

    public void Exit()
    {
        
    }  

    public void Update()
    {
        _timer += Time.deltaTime;

        AnimatorStateInfo state = _enemy.Animator.GetCurrentAnimatorStateInfo(0);

        //모션 끝나면 삭제
        if (state.IsName("Die") && state.normalizedTime >= 1f)
        {
            _enemy.Die();
            return;
        }

        //이중체크ㅡ
        if (_timer >= _dieDuration)
        {
            _enemy.Die();
        }
    }

    public void FixedUpdate()
    {
        if (_enemy.Rigid != null)
            _enemy.Rigid.linearVelocity = Vector2.zero;
    }
}
