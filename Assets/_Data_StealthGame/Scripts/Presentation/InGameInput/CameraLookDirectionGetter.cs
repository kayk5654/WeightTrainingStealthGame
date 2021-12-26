using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// get look direction of the camera transform in the scene
/// </summary>
public class CameraLookDirectionGetter : MonoBehaviour, ILookDirectionGetter
{
    [SerializeField, Tooltip("transform of the main camera")]
    private Transform _cameraTransform;


    /// <summary>
    /// looking direction
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirection()
    {
        return _cameraTransform.forward;
    }

    /// <summary>
    /// position of the viewpoint
    /// </summary>
    /// <returns></returns>
    public Vector3 GetOrigin()
    {
        return _cameraTransform.position;
    }
}
