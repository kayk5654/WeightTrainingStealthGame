using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage ui features
/// </summary>
public class UiManager : IMainMenuStateManager, IGamePlayStateManager
{
    // event to notify the start of MainMenu phase
    public EventHandler<AppStateEventArgs> _onStartMainMenuState { get; }

    // event to notify the start of GamePlay phse
    public EventHandler<AppStateEventArgs> _onStartGamePlayState { get; }

    public void EnableMainMenu()
    {

    }

    public void DisableMainMenu()
    {

    }

    public void EnableGamePlay()
    {

    }

    public void DisableGamePlay()
    {

    }
}
