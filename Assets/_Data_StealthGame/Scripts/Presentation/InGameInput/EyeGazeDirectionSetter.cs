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

    // shader property name for the eye gaze
    private string _eyeGazeDirectionProperty = "_EyeGazeDirection";

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
        //Debug.Log("eye direction = " + _mrtkLookDirection.GetDirection());
    }
}
