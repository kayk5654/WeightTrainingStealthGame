using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// face transform of specific object to the target
/// </summary>
public class LookAtHandler : MonoBehaviour
{
    [SerializeField, Tooltip("transform to control")]
    private Transform _transform;

    [SerializeField, Tooltip("target to look at")]
    private Transform _lookAtTarget;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// update transform
    /// </summary>
    private void Update()
    {
        _transform.LookAt(_lookAtTarget);
    }
}
