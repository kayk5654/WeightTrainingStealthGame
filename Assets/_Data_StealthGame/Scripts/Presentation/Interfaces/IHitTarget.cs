using UnityEngine;
/// <summary>
/// interface of hit target object; spawned projectile can hit this type ofobjects
/// </summary>
public interface IHitTarget
{
    /// <summary>
    /// reaction of the hit object
    /// </summary>
    /// <param name="hitPosition"></param>
    void OnHit(Vector3 hitPosition);
}
