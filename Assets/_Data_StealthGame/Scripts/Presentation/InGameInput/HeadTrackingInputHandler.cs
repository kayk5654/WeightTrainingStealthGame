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

    // Update is called once per frame
    private void Update()
    {
        if (!_isEnabled) { return; }
        UpdatePositionBuffer(_head.position);
        DetectMovmentCycle();
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
    }

    /// <summary>
    /// return end of negative movement
    /// </summary>
    /// <returns></returns>
    public bool IsNegativePeak()
    {
        return false;
    }

    /// <summary>
    /// return start of positive movement
    /// </summary>
    /// <returns></returns>
    public bool IsStartOfPositiveMove()
    {
        return false;
    }

    /// <summary>
    /// return end of positive movement
    /// </summary>
    /// <returns></returns>

    public bool IsEndOfPositivePeak()
    {
        return false;
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
                
                break;

            case MovementPhase.Holding:
                // detect start of positive movement
                
                break;

            case MovementPhase.GoingBackward:
                // detect positive peak of the movment cycle
                
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

        for (int i = 0; i < _movementDetectBuffer.Count - 2; i++)
        {
            _tempVel += (_movementDetectBuffer[i + 1] - _movementDetectBuffer[i]);
        }

        _tempVel /= _movementDetectBuffer.Count - 1;

        return _tempVel;
    }
}
