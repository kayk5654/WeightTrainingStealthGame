using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// input from player's movement while exercises
/// </summary>
public class ExerciseInput : MonoBehaviour, IInGameInputBase, IActionActivator
{
    [SerializeField, Tooltip("tracking target for movement detection")]
    private Transform _trackTarget;

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

    // current phase in the movement cycle
    private MovementPhase _currentMovementPhase = MovementPhase.GoingForward;

    // buffer contains incoming position on each frame to get moving average
    private List<Vector3> _movementDetectBuffer = new List<Vector3>();

    // max length of _movementDetectBuffer
    private int _maxBufferSize = 15;

    // current moving average position of the track target
    private Vector3 _currentMAPosition;

    // current moving average velocity of the track target
    private Vector3 _currentMAVelocity;

    // last moving average velocity of the track target
    private Vector3 _lastMAVelocity;

    /* variables for calculation */

    // store position temporarily
    private Vector3 _tempPos;

    // store velocity temporarily
    private Vector3 _tempVel;

    /*****************************/

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
        }
    }

    /// <summary>
    /// detect input
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }

        UpdatePositionBuffer(_trackTarget.position);
        DetectMovmentCycle();
    }

    /// <summary>
    /// initialize action
    /// </summary>
    public void InitAction()
    {
        // TODO: get exercise input data

        InitBuffer();
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
    /// initialize database to get exercise input type
    /// </summary>
    private void InitDataBase()
    {
        JsonDatabaseReader<ExerciseInputDataSet> exerciseInputJson = new JsonDatabaseReader<ExerciseInputDataSet>();
        _exerciseInputDatabase = new ExerciseInputDatabase(exerciseInputJson, Config._exerciseInputDataPath);
    }

    /// <summary>
    /// init movement detect buffer
    /// </summary>
    private void InitBuffer()
    {
        _movementDetectBuffer.Clear();
    }

    /// <summary>
    /// store incoming position into the movmenent detection buffer for calculation of moving average
    /// </summary>
    /// <param name="position"></param>
    private void UpdatePositionBuffer(Vector3 position)
    {
        if (_movementDetectBuffer.Count >= _maxBufferSize)
        {
            _movementDetectBuffer.RemoveAt(0);
        }

        _movementDetectBuffer.Add(position);
    }

    /// <summary>
    /// detect key points of the movement cycle and kick the relative events
    /// </summary>
    private void DetectMovmentCycle()
    {
        // prepare moving average position/velocity
        _currentMAPosition = GetMovingAveragePosition();
        _currentMAVelocity = GetMovingAverageVelocisy();

        // detect the key points of the movement cycle
        switch (_currentMovementPhase)
        {
            case MovementPhase.GoingForward:
                // detect negative peak of the movement
                if (false)
                {
                    EventArgs args = EventArgs.Empty;
                    _onStartHold?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.Holding;
                }
                break;

            case MovementPhase.Holding:
                // detect start of positive movement
                if (false)
                {
                    EventArgs args = EventArgs.Empty;
                    _onStopHold?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.GoingBackward;
                }
                break;

            case MovementPhase.GoingBackward:
                // detect positive peak of the movment cycle
                if (false)
                {
                    EventArgs args = EventArgs.Empty;
                    _onPush?.Invoke(this, args);
                    _currentMovementPhase = MovementPhase.GoingForward;
                }
                break;

            default:
                break;
        }

        // store moving average velocity for the next frame
        _lastMAVelocity = _currentMAVelocity;
    }

    /// <summary>
    /// get moving average of the incoming position
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMovingAveragePosition()
    {
        _tempPos = Vector3.zero;

        for (int i = 0; i < _movementDetectBuffer.Count; i++)
        {
            _tempPos += _movementDetectBuffer[i];
        }

        _tempPos /= _movementDetectBuffer.Count;

        return _tempPos;
    }

    /// <summary>
    /// get moving average of the incoming velocity
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMovingAverageVelocisy()
    {
        _tempVel = Vector3.zero;

        for(int i = 0; i < _movementDetectBuffer.Count - 2; i++)
        {
            _tempVel += (_movementDetectBuffer[i + 1] - _movementDetectBuffer[i]);
        }

        _tempVel /= _movementDetectBuffer.Count - 1;

        return _tempVel;
    }
}
