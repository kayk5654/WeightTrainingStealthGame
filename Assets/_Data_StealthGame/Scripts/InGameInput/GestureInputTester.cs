using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
public class GestureInputTester : MonoBehaviour, IMixedRealityGestureHandler
{
    /// <summary>
    /// enable to get gesture event
    /// </summary>
    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityGestureHandler>(this);
    }

    /// <summary>
    /// disable to get gesture event
    /// </summary>
    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityGestureHandler>(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGestureCanceled(InputEventData data)
    {

    }

    public void OnGestureCompleted(InputEventData data)
    {

    }

    public void OnGestureStarted(InputEventData data)
    {
        Debug.Log("gesture started");
    }

    public void OnGestureUpdated(InputEventData data)
    {

    }


}
