using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

public class MRTKLookDirectionGetter : MonoBehaviour, ILookDirectionGetter
{
    // eye tracking feature of MRTK
    private IMixedRealityEyeGazeProvider _eyeGazeProvider;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _eyeGazeProvider = CoreServices.InputSystem?.EyeGazeProvider;
        _eyeGazeProvider.IsEyeTrackingEnabled = true;
    }

    /// <summary>
    /// looking direction
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirection()
    {
        return _eyeGazeProvider.GazeDirection.normalized;
    }

    /// <summary>
    /// position of the viewpoint
    /// </summary>
    /// <returns></returns>
    public Vector3 GetOrigin()
    {
        return _eyeGazeProvider.GazeOrigin;
    }
}
