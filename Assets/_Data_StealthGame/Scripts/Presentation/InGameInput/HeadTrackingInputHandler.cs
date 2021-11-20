using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// handle head tracking input to use in ExerciseInput
/// </summary>
public class HeadTrackingInputHandler : MonoBehaviour, IExerciseInputHandler
{
    [SerializeField, Tooltip("tracking target for movement detection")]
    private Transform _head;

    // current exercise input type
    private ExerciseInputDataSet _currentInputType;

    // current phase in the movement cycle
    private MovementPhase _currentMovementPhase = MovementPhase.GoingForward;

    // whether the input is enabled;
    private bool _isEnabled;

    // head position when the exercise starts
    private Vector3 _startHeadPos;

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
    /// initialization of this instance
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// calculate current state
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }
        UpdatePositionBuffer(_head.position);
        CalculateMovingAverage();
    }

    /// <summary>
    /// record data after calculating current state
    /// </summary>
    private void LateUpdate()
    {
        RecordLastMAVelocity();
    }

    /// <summary>
    /// initialization of movement cycle detection
    /// </summary>
    /// <param name="exerciseInputData"></param>
    public void Init(ExerciseInputDataSet exerciseInputData)
    {
        _currentInputType = exerciseInputData;
        InitBuffer();
    }

    /// <summary>
    /// set current movement phase from ExerciseInput
    /// </summary>
    /// <param name="movementPhase"></param>
    public void SetCurrentMovementPhase(MovementPhase movementPhase)
    {
        _currentMovementPhase = movementPhase;
    }

    /// <summary>
    /// enable/disable this exercise input handler
    /// </summary>
    /// <param name="toEnable"></param>
    public void SetEnabled(bool toEnable)
    {
        _isEnabled = toEnable;
        // initialize start head position
        SetStartHeadPosition();
    }

    /// <summary>
    /// return end of negative movement
    /// </summary>
    /// <returns></returns>
    public bool IsNegativePeak()
    {
        if(_currentMovementPhase != MovementPhase.GoingForward) { return false; }

        bool state;

        // detect by position
        if(_currentInputType._peakHeightOffset < 0f)
        {
            state = _head.position.y < _startHeadPos.y + _currentInputType._peakHeightOffset + _currentInputType._heightOffsetMargin;
        }
        else
        {
            state = _head.position.y > _startHeadPos.y + _currentInputType._peakHeightOffset - _currentInputType._heightOffsetMargin;
        }
        

        return state;
    }

    /// <summary>
    /// return start of positive movement
    /// </summary>
    /// <returns></returns>
    public bool IsStartOfPositiveMove()
    {
        if(_currentMovementPhase != MovementPhase.Holding) { return false; }

        bool state;

        // detect by position
        if (_currentInputType._peakHeightOffset < 0f)
        {
            state = _head.position.y > _startHeadPos.y + _currentInputType._peakHeightOffset + _currentInputType._heightOffsetMargin;
        }
        else
        {
            state = _head.position.y < _startHeadPos.y + _currentInputType._peakHeightOffset - _currentInputType._heightOffsetMargin;
        }


        return state;
    }

    /// <summary>
    /// return end of positive movement
    /// </summary>
    /// <returns></returns>

    public bool IsEndOfPositivePeak()
    {
        if(_currentMovementPhase != MovementPhase.GoingBackward) { return false; }

        bool state;

        // detect by position
        if (_currentInputType._peakHeightOffset < 0f)
        {
            state = _head.position.y > _startHeadPos.y - _currentInputType._heightOffsetMargin;
        }
        else
        {
            state = _head.position.y < _startHeadPos.y + _currentInputType._heightOffsetMargin;
        }


        return state;
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
    /// // prepare moving average position/velocity
    /// </summary>
    private void CalculateMovingAverage()
    {
        _currentMAPosition = GetMovingAveragePosition();
        _currentMAVelocity = GetMovingAverageVelocisy();
    }

    /// <summary>
    /// store moving average velocity for the next frame
    /// </summary>
    private void RecordLastMAVelocity()
    {
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

        for (int i = 0; i < _movementDetectBuffer.Count - 2; i++)
        {
            _tempVel += (_movementDetectBuffer[i + 1] - _movementDetectBuffer[i]);
        }

        _tempVel /= _movementDetectBuffer.Count - 1;

        return _tempVel;
    }

    /// <summary>
    /// set start head position to detect movement cycle
    /// </summary>
    private void SetStartHeadPosition()
    {
        _startHeadPos = _head.position;
    }
}
