using System;

/// <summary>
/// state of the whole app
/// </summary>
public enum AppState
{
    MainMenu = 0,
    GamePlay = 1,
}

/// <summary>
/// state of the game play
/// </summary>
public enum GamePlayState
{
    None = 0, // MainMenu phase in the whole app state
    Playing = 1,
    Pausing = 2,
}

/// <summary>
/// notify the status of the app state
/// </summary>
public class AppStateEventArgs : EventArgs
{
    // updated app state
    public AppState appState;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="state"></param>
    public AppStateEventArgs(AppState state)
    {
        appState = state;
    }
}

/// <summary>
/// notify he status of the gameplay state
/// </summary>
public class GamePlayStateEventArgs : EventArgs
{
    // updated gameplay state
    public GamePlayState gamePlayState;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="state"></param>
    public GamePlayStateEventArgs(GamePlayState state)
    {
        gamePlayState = state;
    }
}