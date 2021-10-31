using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// change the app state
/// </summary>
public interface IAppStateSetter
{
    // event to notify the start of MainMenu phase
    event EventHandler<AppStateEventArgs> _onStartMainMenuState;

    // event to notify the start of GamePlay phse
    event EventHandler<AppStateEventArgs> _onStartGamePlayState;

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    void SetAppState(AppState appState);
}
