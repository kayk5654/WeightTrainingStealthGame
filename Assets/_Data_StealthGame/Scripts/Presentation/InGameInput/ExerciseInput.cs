using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// input from player's movement while exercises
/// </summary>
public class ExerciseInput : MonoBehaviour, IInGameInputBase, IActionActivator
{
    // event called when the movement to push player's body up is detected
    public event EventHandler _onPush;

    // event called when the player starts keeping the lowest posture
    public event EventHandler _onStartHold;

    // event called when the player stops keeping the lowest posture
    public event EventHandler _onStopHold;

    // whether the input is enabled;
    private bool _isEnabled;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        InGameInputSwitcher inGameInputSwitcher = FindObjectOfType<InGameInputSwitcher>();
        if (!inGameInputSwitcher) { return; }
        inGameInputSwitcher.SetInputActivator(InputType.exercise, this);
        inGameInputSwitcher.SetInput(InputType.exercise, this);
    }

    /// <summary>
    /// detect input
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }

    }

    /// <summary>
    /// initialize action
    /// </summary>
    public void InitAction()
    {

    }

    /// <summary>
    /// enable action
    /// </summary>
    public void StartAction()
    {
        _isEnabled = true;
    }

    /// <summary>
    /// disable action
    /// </summary>
    public void StopAction()
    {
        _isEnabled = false;
    }
}
