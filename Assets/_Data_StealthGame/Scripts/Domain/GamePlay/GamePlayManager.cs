﻿using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manages features during the gameplay except ui
/// </summary>
public class GamePlayManager : IGamePlayStateManager, IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // manager classes to notice change of the gameplay state
    private IGamePlayStateSetter[] _gameplayStateSetters;



    public GamePlayManager(IGamePlayStateSetter[] gameplayStateSetters)
    {
        _gameplayStateSetters = gameplayStateSetters;
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
    private void UpdateGameplyaState(object sender, GamePlayStateEventArgs args)
    {
        NotifyGamePlayState(args.gamePlayState);
    }

    /// <summary>
    /// send update of the gameplay state to the upper classes in the system
    /// </summary>
    /// <param name="gamePlayState"></param>
    private void NotifyGamePlayState(GamePlayState gamePlayState)
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(gamePlayState);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
