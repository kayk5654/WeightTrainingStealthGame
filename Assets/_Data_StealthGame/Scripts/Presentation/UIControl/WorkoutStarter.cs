using System;
using UnityEngine;
/// <summary>
/// start workout after posture calibration before actual gameplay
/// </summary>
public class WorkoutStarter : MonoBehaviour, IGamePlayStateSetter
{
    [SerializeField, Tooltip("shared references")]
    private SceneObjectContainer _sceneObjectContainer;

    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // look time checker to trigger StartPlay()
    private LookTimeChecker _lookTimeChecker;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lookTimeChecker = _sceneObjectContainer.GetLookTimeChecker();
        _lookTimeChecker._onLookTimeCountEnd += StartPlay;
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _lookTimeChecker._onLookTimeCountEnd -= StartPlay;
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
    public void StartPlay()
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.Playing);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
