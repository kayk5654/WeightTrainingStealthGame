using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage ui features
/// </summary>
public class UiManager : IMainMenuStateManager, IGamePlayStateManager, IAppStateSetter
{
    // event to notify the start of MainMenu phase
    public event EventHandler<AppStateEventArgs> _onStartMainMenuState;

    // event to notify the start of GamePlay phse
    public event EventHandler<AppStateEventArgs> _onStartGamePlayState;


    /// <summary>
    /// enable features at the beginning of the MainMenu phase
    /// </summary>
    public void EnableMainMenu()
    {

    }

    /// <summary>
    /// disable features at the beginning of the MainMenu phase
    /// </summary>
    public void DisableMainMenu()
    {

    }

    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    public void EnableGamePlay()
    {

    }

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    public void DisableGamePlay()
    {

    }

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    public void SetAppState(AppState appState)
    {
        AppStateEventArgs args = new AppStateEventArgs(appState);

        _onStartGamePlayState.Invoke(this, args);
    }
}
