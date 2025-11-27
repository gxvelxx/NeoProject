using System;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    [Header("Player Status")]
    [SerializeField] private int _maxHp = 10;
    [SerializeField] private int _currentHp = 10;
    [SerializeField] private int _attackPower = 1;

    //이벤트
    public event Action<int> OnHealthChanged;
    public event Action OnDie;

    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int AttackPower => _attackPower;

    public void Initialize()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;

        if (_currentHp < 0)
            _currentHp = 0;

        OnHealthChanged?.Invoke(_currentHp);

        if (_currentHp <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public int GetDamage()
    {
        return _attackPower;
    }
}
