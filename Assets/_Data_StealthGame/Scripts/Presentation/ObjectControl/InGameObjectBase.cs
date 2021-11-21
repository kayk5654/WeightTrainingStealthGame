using System;
using UnityEngine;

public abstract class InGameObjectBase : MonoBehaviour
{
    // notify manager of this object that this object is destroyed
    public virtual event EventHandler<InGameObjectEventArgs> _onDestroyed;

    // current remainedHP
    protected float _currentHp;

    // max HP (= fully remained)
    protected float _hp;

    // Attack 
    protected float _attack;

    // Defense
    protected float _defense;

    /// <summary>
    /// destroy this in-game object
    /// </summary>
    public abstract void Destroy();

    /// <summary>
    /// apply damage on this object
    /// </summary>
    /// <param name="damage"></param>
    public abstract void Damage(float damage);

    /// <summary>
    /// apply damage to the target object
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// initialize objects' base parameters
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="attack"></param>
    /// <param name="defense"></param>
    protected void InitParams(float hp, float attack, float defense)
    {
        _hp = hp;
        _currentHp = hp;
        _attack = attack;
        _defense = defense;
    }

    /// <summary>
    /// calculate HP rest after being attacked; damage is applied by a single attack
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    protected float GetRemainedHP_SingleAttack(float damage)
    {
        _currentHp = _currentHp - damage * (1f/ _defense);
        _currentHp = Mathf.Clamp(0f, _hp, _currentHp);
        return _currentHp;
    }

    /// <summary>
    /// calculate HP rest after being attacked; damage is applied continuously
    /// </summary>
    /// <param name="damagePerSecond"></param>
    /// <returns></returns>
    protected float GetRemainedHP_ContinuousAttack(float damagePerSecond)
    {
        _currentHp = _currentHp - damagePerSecond * Time.deltaTime * (1f / _defense);
        _currentHp = Mathf.Clamp(0f, _hp, _currentHp);
        return _currentHp;
    }
}
