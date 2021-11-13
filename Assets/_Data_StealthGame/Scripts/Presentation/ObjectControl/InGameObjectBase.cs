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
}
