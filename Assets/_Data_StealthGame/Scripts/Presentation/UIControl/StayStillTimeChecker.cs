using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// count time that the player keeps static posture
/// then notify when the player stays still longer than the threshold
/// </summary>
public class StayStillTimeChecker : MonoBehaviour, ITimeCountChecker
{
    [SerializeField, Tooltip("shared references")]
    private SceneObjectContainer _sceneObjectContainer;

    // callback when the target object is looked at longer than the threshold
    public event EventHandler _onTimeCountEnd;

    [SerializeField, Tooltip("stay still time threshold")]
    private float _sstayStillTimeThreshold = 3f;

    // look direction getter
    private ILookDirectionGetter _lookDirectionGetter;

    // counted time to look at the target
    private float _stayStillTime;

    // process to count stay still time
    private IEnumerator _stayStillTimeCountSequence;

    // threshold of distance player can move while checking
    private float _distanceThreshold = 0.02f;

    // temporarily store the position of the player's head
    private Vector3 _playerHeadPositionTemp;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lookDirectionGetter = _sceneObjectContainer.GetLookDirectionGetter();
        StartTimeCount();
    }

    /// <summary>
    /// start time counting
    /// </summary>
    private void OnEnable()
    {
        if (_lookDirectionGetter == null) { return; }
        StartTimeCount();
    }

    /// <summary>
    /// reset time count on disable
    /// </summary>
    private void OnDisable()
    {
        StopTimeCount();
        ResetTimeCount();
    }

    /// <summary>
    /// start counting look time
    /// </summary>
    public void StartTimeCount()
    {
        if (_stayStillTimeCountSequence != null)
        {
            return;
        }

        _stayStillTimeCountSequence = StayStillTimeCountSequence();
        StartCoroutine(_stayStillTimeCountSequence);
    }

    /// <summary>
    /// stop counting look time
    /// </summary>
    public void StopTimeCount()
    {
        if (_stayStillTimeCountSequence == null)
        {
            return;
        }

        StopCoroutine(_stayStillTimeCountSequence);
        _stayStillTimeCountSequence = null;
    }

    /// <summary>
    /// reset look time count
    /// </summary>
    public void ResetTimeCount()
    {
        _stayStillTime = 0f;
    }

    /// <summary>
    /// get time phase in the range of 0 to 1
    /// </summary>
    /// <returns></returns>
    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(_stayStillTime / _sstayStillTimeThreshold);
    }

    /// <summary>
    /// count time to stay still
    /// </summary>
    /// <returns></returns>
    private IEnumerator StayStillTimeCountSequence()
    {
        // initialize player's position
        _playerHeadPositionTemp = _lookDirectionGetter.GetOrigin();

        while (_stayStillTime < _sstayStillTimeThreshold)
        {
            // check whether the player stay still
            if(Vector3.Distance(_playerHeadPositionTemp, _lookDirectionGetter.GetOrigin()) < _distanceThreshold)
            {
                _stayStillTime += Time.deltaTime;
            }
            else
            {
                _stayStillTime = 0f;

                _playerHeadPositionTemp = _lookDirectionGetter.GetOrigin();
            }
            
            yield return null;
        }
        
        // notify end of counting
        _onTimeCountEnd?.Invoke(this, EventArgs.Empty);
        _stayStillTimeCountSequence = null;
    }
}
