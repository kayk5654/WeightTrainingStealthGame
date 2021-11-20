using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage ui features
/// </summary>
public class UiManager : IMainMenuStateManager, IGamePlayStateManager, IAppStateSetter, IGamePlayStateSetter, IExerciseInfoSender
{
    // event to notify the start of MainMenu phase
    public event EventHandler<AppStateEventArgs> _onAppStateChange;

    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // notify information about the selected exercise
    public event EventHandler<ExerciseInfoEventArgs> _onExerciseSelected;

    // control main menu ui panel
    private IMultiPhaseUi _menuUi;

    // control workout navigation ui panel during gameplay
    private IMultiPhaseUi _workoutNavigationUi;

    // option menu during gameplay
    private IGamePlayStateSetter _optionMenuUi;

    // cursor ui
    private ICursor _cursorUi;

    // set app state
    private IAppStateSetter _appStateSetter;

    // set exercise info
    private IExerciseInfoSender _exerciseInfoSetter;

    // receive signal to start actual workout
    private IGamePlayStateSetter _workoutStarter;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="menuUi"></param>
    /// <param name="workoutNavUi"></param>
    /// <param name="optionMenuUi"></param>
    /// <param name=""></param>
    public UiManager(IMultiPhaseUi menuUi, IMultiPhaseUi workoutNavUi, IGamePlayStateSetter optionMenuUi, ICursor cursorUi, IAppStateSetter appStateSetter, IExerciseInfoSender exerciseInfoSetter, IGamePlayStateSetter workoutStarter)
    {
        _menuUi = menuUi;
        _workoutNavigationUi = workoutNavUi;
        _optionMenuUi = optionMenuUi;
        _cursorUi = cursorUi;
        _appStateSetter = appStateSetter;
        _exerciseInfoSetter = exerciseInfoSetter;
        _workoutStarter = workoutStarter;
        SetCallback();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    ~UiManager()
    {
        RemoveCallback();
    }

    /// <summary>
    /// enable features at the beginning of the MainMenu phase
    /// </summary>
    public void EnableMainMenu()
    {
        SetAppState(AppState.MainMenu);

        // display root panel of the main menu
        if (_menuUi != null)
        {
            _menuUi.DisplayUiPhase((int)MainUiPanelPhase.Root);
        }
    }

    /// <summary>
    /// disable features at the beginning of the MainMenu phase
    /// </summary>
    public void DisableMainMenu()
    {
        SetAppState(AppState.GamePlay);

        // hide all panel of the main menu
        if (_menuUi != null)
        {
            _menuUi.DisplayUiPhase((int)MainUiPanelPhase.None);
        }
    }

    /// <summary>
    /// enable features for before actual gameplay
    /// </summary>
    public void BeforeGamePlay()
    {
        SetGamePlayState(GamePlayState.BeforePlay);

        // display "before gameplay" phase of the workout navigation ui
        if(_workoutNavigationUi != null)
        {
            _workoutNavigationUi.DisplayUiPhase((int)WorkoutNavigationUiPanelPhase.BeforeGameplay);
        }
    }

    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    public void EnableGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);

        // display the "gameplay" phase of the workout navigation ui
        if(_workoutNavigationUi != null)
        {
            _workoutNavigationUi.DisplayUiPhase((int)WorkoutNavigationUiPanelPhase.Gameplay);
        }

        // display cursor
        if(_cursorUi != null)
        {
            _cursorUi.SetActive(true);
        }
    }

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    public void DisableGamePlay()
    {
        SetGamePlayState(GamePlayState.None);

        // hide all phases of the workout navigation ui
        if (_workoutNavigationUi != null)
        {
            _workoutNavigationUi.DisplayUiPhase((int)WorkoutNavigationUiPanelPhase.None);
        }

        // hide cursor
        if (_cursorUi != null)
        {
            _cursorUi.SetActive(false);
        }
    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    public void PauseGamePlay()
    {
        SetGamePlayState(GamePlayState.Pause);

        // hide cursor
        if (_cursorUi != null)
        {
            _cursorUi.SetActive(false);
        }
    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    public void ResumeGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);

        // display cursor
        if (_cursorUi != null)
        {
            _cursorUi.SetActive(true);
        }
    }

    /// <summary>
    /// enable features for after actual gameplay
    /// </summary>
    public void AfterGamePlay(bool didPlayerWin)
    {
        // display "before gameplay" phase of the workout navigation ui
        if (_workoutNavigationUi != null)
        {
            if (didPlayerWin)
            {
                // if the player wins
                _workoutNavigationUi.DisplayUiPhase((int)WorkoutNavigationUiPanelPhase.GameClear);
            }
            else
            {
                // if the player loses
                _workoutNavigationUi.DisplayUiPhase((int)WorkoutNavigationUiPanelPhase.GameOver);
            }
        }

        SetGamePlayState(GamePlayState.AfterPlay);
    }

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    public void SetAppState(AppState appState)
    {
        
    }

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        if (_optionMenuUi != null)
        {
            _optionMenuUi.SetGamePlayState(gamePlayState);
        }
    }

    /// <summary>
    /// set callback of the ui objects
    /// </summary>
    private void SetCallback()
    {
        if(_optionMenuUi != null)
        {
            _optionMenuUi._onGamePlayStateChange += UpdateGameplyaState;
        }

        if(_appStateSetter != null)
        {
            _appStateSetter._onAppStateChange += UpdateAppState;
        }

        if(_exerciseInfoSetter != null)
        {
            _exerciseInfoSetter._onExerciseSelected += UpdateExerciseInfo;
        }

        if(_workoutStarter != null)
        {
            _workoutStarter._onGamePlayStateChange += UpdateGameplyaState;
        }
    }

    /// <summary>
    /// remove callback of the ui objects
    /// </summary>
    private void RemoveCallback()
    {
        if (_optionMenuUi != null)
        {
            _optionMenuUi._onGamePlayStateChange -= UpdateGameplyaState;
        }

        if (_appStateSetter != null)
        {
            _appStateSetter._onAppStateChange -= UpdateAppState;
        }

        if (_exerciseInfoSetter != null)
        {
            _exerciseInfoSetter._onExerciseSelected -= UpdateExerciseInfo;
        }

        if (_workoutStarter != null)
        {
            _workoutStarter._onGamePlayStateChange -= UpdateGameplyaState;
        }
    }

    /// <summary>
    /// receive update of the gameplay state from the ui, and notice it other classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UpdateGameplyaState(object sender, GamePlayStateEventArgs args)
    {
        NotifyGamePlayState(args._gamePlayState);
    }

    /// <summary>
    /// receive update of the app state from the ui, and notice it other classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UpdateAppState(object sender, AppStateEventArgs args)
    {
        NotifyAppState(args.appState);
    }

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    private void NotifyAppState(AppState appState)
    {
        AppStateEventArgs args = new AppStateEventArgs(appState);
        _onAppStateChange?.Invoke(this, args);
    }

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    private void NotifyGamePlayState(GamePlayState gamePlayState)
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(gamePlayState);
        _onGamePlayStateChange?.Invoke(this, args);
    }

    /// <summary>
    /// update teh exercise info from the lower class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UpdateExerciseInfo(object sender, ExerciseInfoEventArgs args)
    {
        _onExerciseSelected?.Invoke(sender, args);
    }
}
