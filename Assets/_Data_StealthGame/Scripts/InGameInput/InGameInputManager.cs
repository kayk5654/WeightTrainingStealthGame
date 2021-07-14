using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        airTap,
        exercise,
    }

    [SerializeField, Tooltip("type of input to control in-game objects")]
    private InputType _inputType;

    // actual input to control in-game objects
    //private IInGameInput _inGameInput;

    // event called when the movement to push player's body up is detected
    public event EventHandler _onPush;

    // event called when the player starts keeping the lowest posture
    public event EventHandler _onStartHold;

    // event called when the player stops keeping the lowest posture
    public event EventHandler _onStoptHold;

    /// <summary>
    /// initialization of input type
    /// </summary>
    private void Start()
    {
        switch (_inputType)
        {
            case InputType.keyboard:
                break;

            case InputType.airTap:
                break;

            case InputType.exercise:
                break;

            default:
                break;
        }
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
                    _onStoptHold(this, EventArgs.Empty);
                }
                break;

            case InputType.airTap:
                break;

            case InputType.exercise:
                break;

            default:
                break;
        }
    }
}
