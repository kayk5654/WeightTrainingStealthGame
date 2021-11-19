using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manages features during the gameplay except ui
/// </summary>
public class GamePlayManager : IGamePlayStateManager, IGamePlayStateSetter, IExerciseInfoSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // manager classes to notice change of the gameplay state
    private IGamePlayStateSetter[] _gameplayStateSetters;

    // notice exercise type updates
    private IExerciseInfoSetter[] _exerciseInfoSetters;


    public GamePlayManager(IGamePlayStateSetter[] gameplayStateSetters, IExerciseInfoSetter[] exerciseInforSetters)
    {
        _gameplayStateSetters = gameplayStateSetters;
        _exerciseInfoSetters = exerciseInforSetters;

        foreach (IGamePlayStateSetter setter in _gameplayStateSetters)
        {
            setter._onGamePlayStateChange += NotifyGameplyaState;
        }
    }

    /// <summary>
    /// destructor
    /// </summary>
    ~GamePlayManager()
    {
        if (_gameplayStateSetters == null || _gameplayStateSetters.Length < 1) { return; }
        
        foreach (IGamePlayStateSetter setter in _gameplayStateSetters)
        {
            setter._onGamePlayStateChange -= NotifyGameplyaState;
        }
    }

    /// <summary>
    /// enable features for before actual gameplay
    /// </summary>
    public void BeforeGamePlay()
    {
        SetGamePlayState(GamePlayState.BeforePlay);
    }

    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    public void EnableGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);
    }

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    public void DisableGamePlay()
    {
        SetGamePlayState(GamePlayState.None);
    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    public void PauseGamePlay()
    {
        SetGamePlayState(GamePlayState.Pause);
    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    public void ResumeGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);
    }

    /// <summary>
    /// enable features for after actual gameplay
    /// </summary>
    public void AfterGamePlay(bool didPlayerWin)
    {
        SetGamePlayState(GamePlayState.AfterPlay);
    }

    /// <summary>
    /// quit all gameplay features under this class and back to the main menu
    /// </summary>
    public void QuitGamePlay()
    {
        SetGamePlayState(GamePlayState.None);
    }

    /// <summary>
    /// send update of the gameplay state to the lower classes in the execution flow
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        foreach (IGamePlayStateSetter setter in _gameplayStateSetters)
        {
            setter.SetGamePlayState(gamePlayState);
        }
    }

    /// <summary>
    /// receive update of the gameplay state from the lower classes, and notice it upper classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NotifyGameplyaState(object sender, GamePlayStateEventArgs args)
    {
        _onGamePlayStateChange?.Invoke(this, args);
    }

    /// <summary>
    /// set exercise type
    /// </summary>
    /// <param name="exerciseType"></param>
    public void ChangeExerciseType(ExerciseType exerciseType)
    {
        foreach(IExerciseInfoSetter setter in _exerciseInfoSetters)
        {
            setter.ChangeExerciseType(exerciseType);
        }
    }
}
