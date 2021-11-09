using System;
using UnityEngine;

/// <summary>
/// state of the whole app
/// </summary>
public enum AppState
{
    MainMenu = 0,
    GamePlay = 1,
    LENGTH
}

/// <summary>
/// state of the game play
/// </summary>
public enum GamePlayState
{
    None = 0, // MainMenu phase in the whole app state
    Playing = 1,
    Pause = 2,
    LENGTH
}

/// <summary>
/// identify phases of workout navigation ui
/// </summary>
[System.Serializable]
public enum WorkoutNavigationUiPanelPhase
{
    None = -1,
    FormCheck = 0,
    Workout = 1,
    LENGTH
}

/// <summary>
/// names of phases in MainUiPanelController
/// </summary>
[System.Serializable]
public enum MainUiPanelPhase
{
    None = -1,
    Root = 0,
    Tutorial = 1,
    SelectExercise = 2,
    Settings = 3,
    Quit = 4,
    LENGTH,
}

/// <summary>
/// available exercise types
/// </summary>
public enum ExerciseType
{
    None = -1,
    PushUp = 0,
    SitUp = 1,
    Squat = 2,
    ChinUp = 3,
    LENGTH
}

/// <summary>
/// state of enemy object
/// </summary>
public enum EnemyState
{
    Search = 0,
    Attack = 1,
    LENGTH
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

/// <summary>
/// event args forIUiPhase._onMoveToSelectedPhase
/// </summary>
public class UiPhaseEventArgs : EventArgs
{
    // selected phase id to move to
    public int _selectedPhaseId;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="phaseId"></param>
    public UiPhaseEventArgs(int phaseId)
    {
        _selectedPhaseId = phaseId;
    }
}

/// <summary>
/// event args for IExerciseInfoSetter
/// </summary>
public class ExerciseInfoEventArgs : EventArgs
{
    // selected exercise
    public ExerciseType _selectedExercise;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="exerciseType"></param>
    public ExerciseInfoEventArgs(ExerciseType exerciseType)
    {
        _selectedExercise = exerciseType;
    }
}
