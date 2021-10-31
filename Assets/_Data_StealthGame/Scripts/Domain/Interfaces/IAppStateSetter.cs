using System;
/// <summary>
/// change the app state
/// </summary>
public interface IAppStateSetter
{
    // event to notify the start of MainMenu phase
    event EventHandler<AppStateEventArgs> _onAppStateChange;

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    void SetAppState(AppState appState);
}
