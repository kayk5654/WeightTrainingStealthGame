using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// search and find target object to shoot projectiles toward
/// </summary>
public class ProjectileTargetFinder : MonoBehaviour
{

    // target object to shoot projectiles
    private Transform _targetObject;

    // whether the system is searching target object to shoot
    private bool _canFindTarget;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// enable defense action
    /// </summary>
    public void EnableFinding()
    {
        _canFindTarget = true;
        _targetObject = null;
    }

    /// <summary>
    /// disable defense action
    /// </summary>
    public void DisableFinding()
    {
        _canFindTarget = false;
    }

    /// <summary>
    /// set target object
    /// </summary>
    /// <param name="targetObject"></param>
    public void SetTargetObject(Transform targetObject)
    {
        if (!_canFindTarget) { return; }

        // if the given target is null, keep previous reference
        if(targetObject == null) { return; }
        _targetObject = targetObject;
    }

    /// <summary>
    /// provide target object
    /// </summary>
    /// <returns></returns>
    public Transform GetTargetObject()
    {
        if (!_canFindTarget) { return null; }
        return _targetObject;
    }
}
