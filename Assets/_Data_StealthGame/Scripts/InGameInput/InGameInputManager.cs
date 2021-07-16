using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Input;
/// <summary>
/// manages various input to control in-game objects;
/// keyboard, air-tap, exercise movement, etc.
/// for testing purpose
/// </summary>
public class InGameInputManager : MonoBehaviour
{
    // types of input
    [System.Serializable]
    private enum InputType
    {
        keyboard,
        exercise,
        button,
    }

    [SerializeField, Tooltip("type of input to control in-game objects")]
    private InputType _inputType;

    // event called when the movement to push player's body up is detected
    public event EventHandler _onPush;

    // event called when the player starts keeping the lowest posture
    public event EventHandler _onStartHold;

    // event called when the player stops keeping the lowest posture
    public event EventHandler _onStopHold;

    /// <summary>
    /// initialization of input type
    /// </summary>
    private void Start()
    {
        // for debugging, use keyboard on the editor
        // on hololens2, use button instead of keyboard
#if !UNITY_EDITOR
        if(_inputType == InputType.keyboard)
        {
            _inputType = InputType.button;
        }
#endif
    }


    private void Update()
    {
        switch (_inputType)
        {
            case InputType.keyboard:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _onPush(this, EventArgs.Empty);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _onStartHold(this, EventArgs.Empty);
                }

                if (Input.GetKeyUp(KeyCode.Alpha2))
                {
                    _onStopHold(this, EventArgs.Empty);
                }
                break;

            case InputType.exercise:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// execute _onPush fron a button in the scene
    /// </summary>
    public void CallOnPush()
    {
        if(_inputType != InputType.button) { return; }
        _onPush(this, EventArgs.Empty);
    }

    /// <summary>
    /// execute _onStartHold fron a button in the scene
    /// </summary>
    public void CallOnStartHold()
    {
        if (_inputType != InputType.button) { return; }
        _onStartHold(this, EventArgs.Empty);
    }

    /// <summary>
    /// execute _onStopHold fron a button in the scene
    /// </summary>
    public void CallOnStopHold()
    {
        if (_inputType != InputType.button) { return; }
        _onStopHold(this, EventArgs.Empty);
    }
}
