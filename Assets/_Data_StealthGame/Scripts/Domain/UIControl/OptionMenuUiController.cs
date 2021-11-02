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

    }

    /// <summary>
    /// set reference of a option menu in the scene
    /// </summary>
    /// <param name="optionMenuUi"></param>
    public void SetOptionMenuUi(IOptionMenuUi optionMenuUi)
    {
        _optionMenuUi = optionMenuUi;
    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    public void Pause()
    {

    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    public void Resume()
    {

    }

    /// <summary>
    /// terminate gameplay and go back to the main menu
    /// </summary>
    public void BackToMenu()
    {

    }
}
