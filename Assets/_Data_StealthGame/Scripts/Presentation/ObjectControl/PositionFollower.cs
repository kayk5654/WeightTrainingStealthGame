using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// follow another transform
/// </summary>
public class PositionFollower : MonoBehaviour
{
    [SerializeField, Tooltip("tracking target")]
    private Transform _trackingTarget;


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
        transform.position = _trackingTarget.position;
        transform.rotation = _trackingTarget.rotation;
    }
}
