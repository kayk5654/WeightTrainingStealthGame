using System;
using UnityEngine;
/// <summary>
/// start workout after posture calibration before actual gameplay
/// </summary>
public class WorkoutStarter : MonoBehaviour, IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    [SerializeField, Tooltip("look time checker to trigger StartPlay()")]
    private StayStillTimeChecker _timeCountChecker;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _timeCountChecker._onTimeCountEnd += StartPlay;
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _timeCountChecker._onTimeCountEnd -= StartPlay;
    }

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }

    /// <summary>
    /// start workout from a scene object
    /// </summary>
    private void StartPlay(object sender, EventArgs e)
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.Playing);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
