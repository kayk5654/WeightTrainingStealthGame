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

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }
}
