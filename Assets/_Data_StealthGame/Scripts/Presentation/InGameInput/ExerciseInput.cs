using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// input from player's movement while exercises
/// </summary>
public class ExerciseInput : MonoBehaviour, IInGameInputBase, IActionActivator, IExerciseInfoSetter
{
    // event called when the movement to push player's body up is detected
    public event EventHandler _onPush;

    // event called when the player starts keeping the lowest posture
    public event EventHandler _onStartHold;

    // event called when the player stops keeping the lowest posture
    public event EventHandler _onStopHold;

    // whether the input is enabled;
    private bool _isEnabled;

    // database to get exercise input type
    private ExerciseInputDatabase _exerciseInputDatabase;

    // get exercise input
    private IExerciseInputHandler _exerciseInputHandler;

    // current exercise input type
    private ExerciseInputDataSet _currentInputType;

    [SerializeField, Tooltip("head tracking exercise input")]
    private HeadTrackingInputHandler _headTracking;

    // current phase in the movement cycle
    private MovementPhase _currentMovementPhase = MovementPhase.GoingForward;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        InitDataBase();

        InGameInputSwitcher inGameInputSwitcher = FindObjectOfType<InGameInputSwitcher>();
        if (inGameInputSwitcher) 
        {
            inGameInputSwitcher.SetInputActivator(InputType.Exercise, this);
            inGameInputSwitcher.SetInput(InputType.Exercise, this);
            inGameInputSwitcher.SetExerciseInfoSetter(this);
        }
    }

    /// <summary>
    /// detect input
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }
        DetectMovmentCycle();
    }

    /// <summary>
    /// initialize action
    /// </summary>
    public void InitAction()
    {
        _currentMovementPhase = MovementPhase.GoingForward;

        // set up appropriate exercise input handler
        InitExerciseInput();
        _exerciseInputHandler.Init(_currentInputType);
        _exerciseInputHandler.SetCurrentMovementPhase(MovementPhase.GoingForward);
    }

    /// <summary>
    /// enable action
    /// </summary>
    public void StartAction()
    {
        _isEnabled = true;
        _exerciseInputHandler.SetEnabled(_isEnabled);
    }

    /// <summary>
    /// disable action
    /// </summary>
    public void StopAction()
    {
        _isEnabled = false;
        _exerciseInputHandler.SetEnabled(_isEnabled);
    }

    /// <summary>
    /// set exercise type
    /// </summary>
    /// <param name="exerciseType"></param>
    public void ChangeExerciseType(ExerciseType exerciseType)
    {
        if (_exerciseInputDatabase == null) { return; }
        _currentInputType = _exerciseInputDatabase.GetData((int)exerciseType);
    }

    /// <summary>
    /// initialize database to get exercise input type
    /// </summary>
    private void InitDataBase()
    {
        JsonDatabaseReader<ExerciseInputDataSet> exerciseInputJson = new JsonDatabaseReader<ExerciseInputDataSet>();
        _exerciseInputDatabase = new ExerciseInputDatabase(exerciseInputJson, Config._exerciseInputDataPath);
    }

    /// <summary>
    /// initialize exercise input for each type of exercises
    /// </summary>
    private void InitExerciseInput()
    {
        switch (_currentInputType._inputType)
        {
            case ExerciseInputType.HeadTracking:
                _exerciseInputHandler = _headTracking;
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// detect key points of the movement cycle and kick the relative events
    /// </summary>
    private void DetectMovmentCycle()
    {
        if(_exerciseInputHandler == null) { return; }
        
        // detect the key points of the movement cycle
        switch (_currentMovementPhase)
        {
            case MovementPhase.GoingForward:
                // detect negative peak of the movement
                if (_exerciseInputHandler.IsNegativePeak())
                {
                    EventArgs args = EventArgs.Empty;
                    _onStartHold?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.Holding;
                    _exerciseInputHandler.SetCurrentMovementPhase(_currentMovementPhase);
                }
                break;

            case MovementPhase.Holding:
                // detect start of positive movement
                if (_exerciseInputHandler.IsStartOfPositiveMove())
                {
                    EventArgs args = EventArgs.Empty;
                    _onStopHold?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.GoingBackward;
                    _exerciseInputHandler.SetCurrentMovementPhase(_currentMovementPhase);
                }
                break;

            case MovementPhase.GoingBackward:
                // detect positive peak of the movment cycle
                if (_exerciseInputHandler.IsEndOfPositivePeak())
                {
                    EventArgs args = EventArgs.Empty;
                    _onPush?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.GoingForward;
                    _exerciseInputHandler.SetCurrentMovementPhase(_currentMovementPhase);
                }
                break;

            default:
                break;
        }
    }

}
