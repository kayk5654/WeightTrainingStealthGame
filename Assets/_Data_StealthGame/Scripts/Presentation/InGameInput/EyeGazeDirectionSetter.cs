using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// set looking direction to the shaders
/// </summary>
public class EyeGazeDirectionSetter : MonoBehaviour
{
    [SerializeField, Tooltip("look direction getter")]
    private MRTKLookDirectionGetter _mrtkLookDirection;

    [SerializeField, Tooltip("get camera forward direction")]
    private CameraLookDirectionGetter _cameraLookDirectionGetter;

    // shader property name for the eye gaze
    private string _eyeGazeDirectionProperty = "_EyeGazeDirection";

    // shadre property name for the rotation matrix based on the eye gaze
    private string _eyeGazeSpaceMatrixProperty = "_EyeGazeSpaceMatrix";

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// update looking direction
    /// </summary>
    private void Update()
    {
        Shader.SetGlobalVector(_eyeGazeDirectionProperty, _mrtkLookDirection.GetDirection());
        //Shader.SetGlobalMatrix(_eyeGazeSpaceMatrixProperty, Matrix4x4.TRS(_mrtkLookDirection.GetOrigin(), Quaternion.FromToRotation(_cameraLookDirectionGetter.GetDirection(), _mrtkLookDirection.GetDirection()), Vector3.one));
        //Debug.Log("eye direction = " + _mrtkLookDirection.GetDirection());
    }
}
