﻿using System.Collections;
using System;
using UnityEngine;
/// <summary>
/// count time to look at a specific object,
/// then notify when the target object is looked at longer than the threshold
/// </summary>
public class LookTimeChecker : MonoBehaviour, ITimeCountChecker
{
    [SerializeField, Tooltip("shared references")]
    private SceneObjectContainer _sceneObjectContainer;

    // callback when the target object is looked at longer than the threshold
    public event EventHandler _onTimeCountEnd;

    [SerializeField, Tooltip("look time threshold")]
    private float _lookTimeThreshold = 4f;

    [SerializeField, Tooltip("target object to look at")]
    private Transform _targetObject;

    // look direction getter
    private ILookDirectionGetter _lookDirectionGetter;

    // counted time to look at the target
    private float _lookTime;

    // process to count look time
    private IEnumerator _lookTimeCountSequence;

    // dot threshold to compare look direction and head-to-target vector
    private float _dotThreshold = 0.9f;



    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lookDirectionGetter = _sceneObjectContainer.GetLookDirectionGetter();
        StartTimeCount();
    }

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
        if(_lookTimeCountSequence != null)
        {
            return;
        }

        _lookTimeCountSequence = LookTimeCountSequence();
        StartCoroutine(_lookTimeCountSequence);
    }

    /// <summary>
    /// stop counting look time
    /// </summary>
    public void StopTimeCount()
    {
        if(_lookTimeCountSequence == null)
        {
            return;
        }

        StopCoroutine(_lookTimeCountSequence);
        _lookTimeCountSequence = null;
    }

    /// <summary>
    /// reset look time count
    /// </summary>
    public void ResetTimeCount()
    {
        _lookTime = 0f;
    }

    /// <summary>
    /// count look time
    /// </summary>
    /// <returns></returns>
    private IEnumerator LookTimeCountSequence()
    {
        while(_lookTime < _lookTimeThreshold)
        {
            if(Vector3.Dot(_lookDirectionGetter.GetDirection(), (_targetObject.position - _lookDirectionGetter.GetOrigin()).normalized) > _dotThreshold)
            {
                _lookTime += Time.deltaTime;
            }
            else
            {
                _lookTime -= Time.deltaTime * 0.5f;
                _lookTime = Mathf.Max(_lookTime, 0f);
            }
            
            yield return null;
        }

        // notify end of counting
        _onTimeCountEnd?.Invoke(this, EventArgs.Empty);
        _lookTimeCountSequence = null;
    }

    /// <summary>
    /// get look time phase in the range of 0 to 1
    /// </summary>
    /// <returns></returns>
    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(_lookTime / _lookTimeThreshold);
    }
}
