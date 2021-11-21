using System;
using UnityEngine;

public abstract class InGameObjectBase : MonoBehaviour
{
    // notify manager of this object that this object is destroyed
    public virtual event EventHandler<InGameObjectEventArgs> _onDestroyed;

    // HP
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
}
