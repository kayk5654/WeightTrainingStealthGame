using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// search and find target object to shoot projectiles toward
/// </summary>
public class ProjectileTargetFinder : MonoBehaviour
{
    [SerializeField, Tooltip("target focus guide")]
    private GameObject _focusGuide;

    // target object to shoot projectiles
    private Transform _targetObject;

    // whether the system is searching target object to shoot
    private bool _canFindTarget;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        SetFocusGuideStatus();
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
        _focusGuide.SetActive(false);
    }

    /// <summary>
    /// set target object
    /// </summary>
    /// <param name="targetObject"></param>
    public void SetTargetObject(Transform targetObject)
    {
        if (!_canFindTarget) { return; }

        // if the given target is null, keep previous reference
        if(targetObject == null) {  return; }
        _targetObject = targetObject;
    }

    /// <summary>
    /// provide target object
    /// </summary>
    /// <returns></returns>
    public Transform GetTargetObject()
    {
        // return the object that was watched last time
        return _targetObject;
    }

    /// <summary>
    /// display/hide focus guide depending on the status of _targetObject
    /// </summary>
    private void SetFocusGuideStatus()
    {
        if (!_canFindTarget) 
        {
            // hide focus guide
            _focusGuide.SetActive(false);
            return; 
        }

        if (_targetObject)
        {
            // display focus guide
            if (!_focusGuide.activeSelf)
            {
                _focusGuide.SetActive(true);
            }
            // set transform of the focus guide
            _focusGuide.transform.position = _targetObject.transform.position;
        }
        else
        {
            // hide focus guide
            _focusGuide.SetActive(false);
        }
    }
}
