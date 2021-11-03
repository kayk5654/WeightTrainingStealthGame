using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage ui features
/// </summary>
public class UiManager : IMainMenuStateManager, IGamePlayStateManager, IAppStateSetter, IGamePlayStateSetter
{
    // event to notify the start of MainMenu phase
    public event EventHandler<AppStateEventArgs> _onAppStateChange;

    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // control main menu ui panel
    private IMultiPhaseUi _menuUi;

    // control workout navigation ui panel during gameplay
    private IMultiPhaseUi _workoutNavigationUi;

    // option menu during gameplay
    private IGamePlayStateSetter _optionMenuUi;

    // cursor ui
    private ICursor _cursorUi;


    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="menuUi"></param>
    /// <param name="workoutNavUi"></param>
    /// <param name="optionMenuUi"></param>
    /// <param name=""></param>
    public UiManager(IMultiPhaseUi menuUi, IMultiPhaseUi workoutNavUi, IGamePlayStateSetter optionMenuUi, ICursor cursorUi)
    {
        _menuUi = menuUi;
        _workoutNavigationUi = workoutNavUi;
        _optionMenuUi = optionMenuUi;
        _cursorUi = cursorUi;
        SetCallback();
    }

    /// <summary>
    /// enable features at the beginning of the MainMenu phase
    /// </summary>
    public void EnableMainMenu()
    {
        SetAppState(AppState.MainMenu);
    }

    /// <summary>
    /// disable features at the beginning of the MainMenu phase
    /// </summary>
    public void DisableMainMenu()
    {
        SetAppState(AppState.GamePlay);
    }

    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    public void EnableGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);
    }

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    public void DisableGamePlay()
    {
        SetGamePlayState(GamePlayState.None);
    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    public void PauseGamePlay()
    {
        SetGamePlayState(GamePlayState.Pause);
    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    public void ResumeGamePlay()
    {
        SetGamePlayState(GamePlayState.Playing);
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
    }

    /// <summary>
    /// receive update of the gameplay state from the ui, and notice it other classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UpdateGameplyaState(object sender, GamePlayStateEventArgs args)
    {
        NotifyGamePlayState(args.gamePlayState);
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
}
