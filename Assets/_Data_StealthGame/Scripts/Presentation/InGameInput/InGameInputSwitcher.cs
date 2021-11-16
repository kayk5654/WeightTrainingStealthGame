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
public class InGameInputSwitcher : MonoBehaviour, IActionActivator, IInGameInputBase, IExerciseInfoSetter
{
    

    [SerializeField, Tooltip("type of input to control in-game objects")]
    private InputType _inputType;

    // event called when the movement to push player's body up is detected
    public event EventHandler _onPush;

    // event called when the player starts keeping the lowest posture
    public event EventHandler _onStartHold;

    // event called when the player stops keeping the lowest posture
    public event EventHandler _onStopHold;

    // input from buttons
    private IInGameInputBase _buttonInput;

    // input from keyboard
    private IInGameInputBase _keyboardInput;

    // input from player's workout
    private IInGameInputBase _exerciseInput;

    // send exercise type if exercise input is used
    private IExerciseInfoSetter _exerciseInfoSetter;

    private Dictionary<InputType, IActionActivator> _inputActivators = new Dictionary<InputType, IActionActivator>();

    // whether the input is enabled;
    private bool _isEnabled;



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

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        if(_keyboardInput != null)
        {
            _keyboardInput._onPush -= OnPush;
            _keyboardInput._onStartHold -= OnStartHold;
            _keyboardInput._onStopHold -= OnStopHold;
        }

        if (_buttonInput != null)
        {
            _buttonInput._onPush -= OnPush;
            _buttonInput._onStartHold -= OnStartHold;
            _buttonInput._onStopHold -= OnStopHold;
        }

        if (_exerciseInput != null)
        {
            _exerciseInput._onPush -= OnPush;
            _exerciseInput._onStartHold -= OnStartHold;
            _exerciseInput._onStopHold -= OnStopHold;
        }
    }

    #region IActionActivator

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
        foreach(InputType key in _inputActivators.Keys)
        {
            if(key != _inputType) { continue; }
            _inputActivators[key].StartAction();
        }
    }

    /// <summary>
    /// disable action
    /// </summary>
    public void StopAction()
    {
        _isEnabled = false;
        foreach (InputType key in _inputActivators.Keys)
        {
            if (key != _inputType) { continue; }
            _inputActivators[key].StopAction();
        }
    }

    #endregion

    /// <summary>
    /// set exercise type
    /// </summary>
    /// <param name="exerciseType"></param>
    public void ChangeExerciseType(ExerciseType exerciseType)
    {
        if (_exerciseInfoSetter == null) { return; }
        _exerciseInfoSetter.ChangeExerciseType(exerciseType);
    }

    /// <summary>
    /// set actual input
    /// </summary>
    /// <param name="inputType"></param>
    /// <param name="input"></param>
    public void SetInput(InputType inputType, IInGameInputBase input)
    {
        switch (inputType)
        {
            case InputType.Keyboard:
                _keyboardInput = input;
                break;

            case InputType.Button:
                _buttonInput = input;
                break;

            case InputType.Exercise:
                _exerciseInput = input;
                break;

            default:
                break;
        }

        // initialize callbacks
        InitInputCallback(input);
    }

    /// <summary>
    /// add input activator
    /// </summary>
    /// <param name="activator"></param>
    public void SetInputActivator(InputType inputType, IActionActivator activator)
    {
        if (_inputActivators.ContainsKey(inputType)) { return; }
        _inputActivators.Add(inputType, activator);
    }

    /// <summary>
    /// set reference of exercise info setter
    /// </summary>
    /// <param name="exerciseInfoSetter"></param>
    public void SetExerciseInfoSetter(IExerciseInfoSetter exerciseInfoSetter)
    {
        _exerciseInfoSetter = exerciseInfoSetter;
    }

    /// <summary>
    /// execute _onPush fron a button in the scene
    /// </summary>
    private void OnPush(object sender, EventArgs args)
    {
        if(_onPush == null) { return; }
        _onPush(sender, args);
    }

    /// <summary>
    /// execute _onStartHold fron a button in the scene
    /// </summary>
    private void OnStartHold(object sender, EventArgs args)
    {
        if(_onStartHold == null) { return; }
        _onStartHold(sender, args);
    }

    /// <summary>
    /// execute _onStopHold fron a button in the scene
    /// </summary>
    private void OnStopHold(object sender, EventArgs args)
    {
        if(_onStopHold == null) { return; }
        _onStopHold(sender, args);
    }

    /// <summary>
    /// initialize callback on the selected input type
    /// </summary>
    private void InitInputCallback(IInGameInputBase input)
    {
        input._onPush += OnPush;
        input._onStartHold += OnStartHold;
        input._onStopHold += OnStopHold;
    }

}
