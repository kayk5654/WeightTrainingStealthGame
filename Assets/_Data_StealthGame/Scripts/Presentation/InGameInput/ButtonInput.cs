using System;
using UnityEngine;
/// <summary>
/// input from buttons with UnityEvent
/// </summary>
public class ButtonInput : MonoBehaviour, IInGameInputBase, IActionActivator
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
        inGameInputSwitcher.SetInputActivator(InputType.Button, this);
        inGameInputSwitcher.SetInput(InputType.Button, this);
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

    /// <summary>
    /// execute _onPush fron a button in the scene
    /// </summary>
    public void OnPush()
    {
        if (!_isEnabled) { return; }
        EventArgs args = EventArgs.Empty;
        _onPush(this, args);
    }

    /// <summary>
    /// execute _onStartHold fron a button in the scene
    /// </summary>
    public void OnStartHold()
    {
        if (!_isEnabled) { return; }
        EventArgs args = EventArgs.Empty;
        _onStartHold(this, args);
    }

    /// <summary>
    /// execute _onStopHold fron a button in the scene
    /// </summary>
    public void OnStopHold()
    {
        if (!_isEnabled) { return; }
        EventArgs args = EventArgs.Empty;
        _onStopHold(this, args);
    }
}
