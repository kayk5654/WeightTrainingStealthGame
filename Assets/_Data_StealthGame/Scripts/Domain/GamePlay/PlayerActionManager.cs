using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage player's actions during the gameplay
/// </summary>
public class PlayerActionManager : IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // last gameplay state
    private GamePlayState _lastGameplayState = GamePlayState.None;

    // player input actions to enable/disable
    private IActionActivator[] _actionActivators;

    // offense action during gameplay by the player
    private IInGameOffenseAction _offenseAction;

    // defense action during gameplay by the player
    private IInGameDefenseAction _defenseAction;


    /// <summary>
    /// set offense action during gameplay
    /// </summary>
    /// <param name="offenseAction"></param>
    public void SetOffenseAction(IInGameOffenseAction offenseAction)
    {
        _offenseAction = offenseAction;
    }

    /// <summary>
    /// set defense action during gameplay
    /// </summary>
    /// <param name="defenseAction"></param>
    public void SetDefenseAction(IInGameDefenseAction defenseAction)
    {
        _defenseAction = defenseAction;
    }

    /// <summary>
    /// set reference of actions that are enabled/disabled by this class
    /// </summary>
    /// <param name="actionActivators"></param>
    public void SetActionActivators(IActionActivator[] actionActivators)
    {
        _actionActivators = actionActivators;
    }

    /// <summary>
    /// send update of the gameplay state to the lower classes in the execution flow
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        switch (gamePlayState)
        {
            case GamePlayState.None:
                // if the gameplay is terminated, stop player input
                StopPlayerInput();
                break;

            case GamePlayState.Playing:
                if (_lastGameplayState == GamePlayState.None)
                {
                    // if the gameplay starts, initialize and start player input
                    InitPlayerInput();
                    StartPlayerInput();
                }
                else if (_lastGameplayState == GamePlayState.Pause)
                {
                    // if the gameplay is resumed, resume player input
                    StartPlayerInput();
                }
                break;

            case GamePlayState.Pause:
                // stop player input
                StopPlayerInput();
                break;

            default:
                break;
        }

        // record gameplay state
        _lastGameplayState = gamePlayState;
    }

    /// <summary>
    /// initialize player input
    /// </summary>
    private void InitPlayerInput()
    {
        if(_actionActivators == null || _actionActivators.Length < 1) { return; }
        
        foreach(IActionActivator activator in _actionActivators)
        {
            activator.InitAction();
        }
    }

    /// <summary>
    /// start player input
    /// </summary>
    private void StartPlayerInput()
    {
        if (_actionActivators == null || _actionActivators.Length < 1) { return; }

        foreach (IActionActivator activator in _actionActivators)
        {
            activator.StartAction();
        }
    }

    /// <summary>
    /// stop player input
    /// </summary>
    private void StopPlayerInput()
    {
        if (_actionActivators == null || _actionActivators.Length < 1) { return; }

        foreach (IActionActivator activator in _actionActivators)
        {
            activator.StopAction();
        }
    }
}
