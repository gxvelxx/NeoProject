using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitboxData
{
    public Vector2 offset; // 콜라이더 위치
    public Vector2 size;   // 콜라이더 크기
}

[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{    
    public Collider2D _hitCollider;

    [Header("Hitbox Data")]
    private HitboxData[] comboData;

    public int _damage = 1;

    private bool _active = false;
    private HashSet<int> _alreadyHitIds = new HashSet<int>();

    private void Reset()
    {
        if (_hitCollider == null)
            _hitCollider = GetComponent<Collider2D>();
        if (_hitCollider != null)
            _hitCollider.isTrigger = true;
    }

    private void Awake()
    {
        if (_hitCollider == null)
            _hitCollider= GetComponent<Collider2D>();
        if (_hitCollider != null)
        {
            _hitCollider.isTrigger = true;
            _hitCollider.enabled = false;
        }
    }
    
    public void EnableHitbox()
    {
        _alreadyHitIds.Clear();
        _active = true;
        if (_hitCollider != null)
            _hitCollider.enabled = true;
    }
    
    public void DisableHitbox()
    {
        _active = false;
        if (_hitCollider != null)
            _hitCollider.enabled = false;
    }

    public void ApplyHitbox(int step, bool facingRight)
    {
        SetDirection(facingRight);
        SetComboStep(step);
    }

    public void SetComboStep(int step)
    {
        BoxCollider2D box = _hitCollider as BoxCollider2D;

        if (box == null || comboData == null || comboData.Length == 0)
            return;

        //범위 초과 방지
        step = Mathf.Clamp(step, 0, comboData.Length - 1);

        //데이터 적용
        box.offset = comboData[step].offset;
        box.size = comboData[step].size;
    }
    
    //히트박스 플레이어 시선따라
    public void SetDirection(bool facingRight)
    {
        if (_hitCollider == null)
            return;
        
        Vector3 pos = transform.localPosition;

        //오른쪽이면 +, 왼쪽이면 -
        float sign = facingRight ? 1f : -1f;

        //x 위치만 반전
        pos.x = Mathf.Abs(pos.x) * sign;
        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_active)
            return;        

        Debug.Log($"충돌생김: {collision.gameObject.name}");

        //중복 안되게
        int id = collision.GetInstanceID();
        if (_alreadyHitIds.Contains(id))
            return;

        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            Debug.Log("적 맞음!");
            _alreadyHitIds.Add(id);
           
            enemy.TakeDamage(_damage);

            HitStopManager.Instance?.PlayHitStop(0.06f);
        }
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
