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
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(gamePlayState);
        _onGamePlayStateChange?.Invoke(this, args);
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
        _optionMenuUi._onPause += Pause;
        _optionMenuUi._onResume += Resume;
        _optionMenuUi._onBackToMenu += BackToMenu;
    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    private void Pause(object sender, EventArgs args)
    {
        SetGamePlayState(GamePlayState.Pausing);
    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    private void Resume(object sender, EventArgs args)
    {
        SetGamePlayState(GamePlayState.Playing);
    }

    /// <summary>
    /// terminate gameplay and go back to the main menu
    /// </summary>
    private void BackToMenu(object sender, EventArgs args)
    {
        SetGamePlayState(GamePlayState.None);
    }
}
