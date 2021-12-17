using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// count time of gameplay, and notify if it reaches at the time limit
/// </summary>
public class TimeLimitCounter : MonoBehaviour, IPlayTimeCounter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayEnd;

    // notify the time count reaches at the time to activate features for last several seconds of the gameplay
    public event EventHandler _lastRushTimeEvent;

    // current play time of this play
    private float _currentPlayTime;

    // time limit of the gameplay
    private float _timeLimit;

    // time to activate features in the remained time defined by its argument
    private float _lastRushTime;

    // whether _lastRushTimeEvent is executed during current gameplay
    private bool _isLastRushTimeEventCalled;

    private IEnumerator _timeCountSequence;



    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }

    /// <summary>
    /// set time limit
    /// </summary>
    /// <param name="timeLimit"></param>
    public void SetTimeLimit(float timeLimit)
    {
        _timeLimit = timeLimit;
    }

    /// <summary>
    /// start counting playtime
    /// </summary>
    public void StartCount()
    {
        _currentPlayTime = 0f;
        _isLastRushTimeEventCalled = false;

        _timeCountSequence = TimeCountSequence();
        StartCoroutine(_timeCountSequence);
    }

    /// <summary>
    /// temporarily resume counting playtime
    /// </summary>
    public void PauseCount()
    {
        if(_timeCountSequence != null)
        {
            StopCoroutine(_timeCountSequence);
        }
    }

    /// <summary>
    /// resume counting playtime
    /// </summary>
    public void ResumeCount()
    {
        if (_timeCountSequence != null)
        {
            StartCoroutine(_timeCountSequence);
        }
    }

    /// <summary>
    /// terminate counting playtime
    /// </summary>
    public void QuitCount()
    {
        if (_timeCountSequence != null)
        {
            StopCoroutine(_timeCountSequence);
            _timeCountSequence = null;
        }
    }

    /// <summary>
    /// send the end of the gameplay
    /// </summary>
    private void SendGamePlayEnd()
    {
        GamePlayStateEventArgs gamePlayStateargs = new GamePlayStateEventArgs(GamePlayState.AfterPlay);
        _onGamePlayEnd?.Invoke(this, gamePlayStateargs);
    }

    /// <summary>
    /// count time process
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeCountSequence()
    {
        while (true)
        {
            _currentPlayTime += Time.deltaTime;

            // execute _lastRushTimeEvent
            if (IsLastRushTime() && !_isLastRushTimeEventCalled)
            {
                _lastRushTimeEvent?.Invoke(this, EventArgs.Empty);
                _isLastRushTimeEventCalled = true;
            }

            if(_currentPlayTime >= _timeLimit)
            {
                SendGamePlayEnd();
                _timeCountSequence = null;
                yield break;
            }
            
            yield return null;
        }
    }

    /// <summary>
    /// set time to activate features in the remained time defined by its argument
    /// </summary>
    /// <param name="lastRushTime"></param>
    public void SetLastRushTime(float lastRushTime)
    {
        _lastRushTime = lastRushTime;
    }

    /// <summary>
    /// check whether it's already in the last several seconds defined in SetLastRushTime()
    /// </summary>
    /// <returns></returns>
    private bool IsLastRushTime()
    {
        return _timeLimit - _currentPlayTime <= _lastRushTime;
    }
}
