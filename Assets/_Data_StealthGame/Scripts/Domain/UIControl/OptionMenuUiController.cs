using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// control option menu ui during gameplay
/// </summary>
public class OptionMenuUiController : IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    private IOptionMenuUi _optionMenuUi;


    /// <summary>
    /// remove callback
    /// </summary>
    ~OptionMenuUiController()
    {
        if (_optionMenuUi != null) 
        {
            _optionMenuUi._onGameplayStateChange -= UpdateGameplyaState;
        }
        
    }

    /// <summary>
    /// receive update of the gameplay state from the upper class of the flow of the system
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        // depending on the updated state, turn on/off the option ui
        if(gamePlayState == GamePlayState.None)
        {
            _optionMenuUi.EnableUi(false);
        }
        else
        {
            _optionMenuUi.EnableUi(true);
        }
    }

    /// <summary>
    /// set reference of a option menu in the scene
    /// </summary>
    /// <param name="optionMenuUi"></param>
    public void SetOptionMenuUi(IOptionMenuUi optionMenuUi)
    {
        _optionMenuUi = optionMenuUi;
        SetUiCallback();
    }

    /// <summary>
    /// set pause/resume/back to menu as ui callbacks
    /// </summary>
    private void SetUiCallback()
    {
        if(_optionMenuUi == null) { return; }
        _optionMenuUi._onGameplayStateChange += UpdateGameplyaState;
    }

    /// <summary>
    /// receive update of the gameplay state from the ui, and notice it other classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UpdateGameplyaState(object sender, GamePlayStateEventArgs args)
    {
        NotifyGamePlayState(args._gamePlayState);
    }

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    private void NotifyGamePlayState(GamePlayState gamePlayState)
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(gamePlayState);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
