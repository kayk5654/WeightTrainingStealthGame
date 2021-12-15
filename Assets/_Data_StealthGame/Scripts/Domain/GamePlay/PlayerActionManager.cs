using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage player's actions during the gameplay
/// </summary>
public class PlayerActionManager : IGamePlayStateSetter, IExerciseInfoSetter
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

    // receive player's input
    private IInGameInputBase _input;

    // send exercise type
    private IExerciseInfoSetter _exerciseInfoSetter;


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
    /// set reference of in-game input
    /// </summary>
    /// <param name="input"></param>
    public void SetInGameInput(IInGameInputBase input)
    {
        _input = input;
    }

    /// <summary>
    /// set reference of exercise info setter
    /// </summary>
    /// <param name="exerciseInfoSetter"></param>
    public void SetExerciseInfoSetter(IExerciseInfoSetter exerciseInfoSetter)
    {
        _exerciseInfoSetter = exerciseInfoSetter;
    }

    /// <summary>
    /// set callback between input and actions
    /// </summary>
    public void SetCallback()
    {
        if(_input == null) { return; }

        // set offense action callback
        if(_offenseAction != null)
        {
            _input._onPush += StartOffenseAction;
        }

        // set defense action callback
        if(_defenseAction != null)
        {
            _input._onStartHold += StartDefenseAction;
            _input._onStopHold += StopDefenseAction;
        }
    }

    /// <summary>
    /// remove callback between input and actions
    /// </summary>
    public void RemoveCallback()
    {
        if (_input == null) { return; }

        // set offense action callback
        if (_offenseAction != null)
        {
            _input._onPush -= StartOffenseAction;
        }

        // set defense action callback
        if (_defenseAction != null)
        {
            _input._onStartHold -= StartDefenseAction;
            _input._onStopHold -= StopDefenseAction;
        }
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

            case GamePlayState.BeforePlay:
                // if the gameplay starts, initialize and start player input
                InitPlayerInput();
                break;

            case GamePlayState.Playing:
                // start player input
                StartPlayerInput();
                break;

            case GamePlayState.Pause:
                // stop player input
                StopPlayerInput();
                break;

            case GamePlayState.AfterPlay:
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
    /// set exercise type
    /// </summary>
    /// <param name="exerciseType"></param>
    public void ChangeExerciseType(ExerciseType exerciseType)
    {
        _exerciseInfoSetter.ChangeExerciseType(exerciseType);
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

    /// <summary>
    /// kick offenseive action from an event of the input class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void StartOffenseAction(object sender, EventArgs args)
    {
        if(_offenseAction == null) { return; }
        _offenseAction.Attack();
        DebugLog.Info(this.ToString(), "Attack");
    }

    /// <summary>
    /// start defensive action from an event of the input class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void StartDefenseAction(object sender, EventArgs args)
    {
        if(_defenseAction == null) { return; }
        _defenseAction.StartDefense();
    }

    /// <summary>
    /// stop defensive action from an event of the input class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void StopDefenseAction(object sender, EventArgs args)
    {
        if (_defenseAction == null) { return; }
        _defenseAction.StopDefense();
    }
}
