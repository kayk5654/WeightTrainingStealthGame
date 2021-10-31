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