using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Manage the phases of this app
/// </summary>
public class AppManager
{
    // current app state; initial app state is MainMenu
    private AppState _currentAppState = AppState.MainMenu;

    // current gameplay state; initial state is None (= MainMenu)
    private GamePlayState _currentGamePlayState = GamePlayState.None;

    // start and quit app in the unity's scene; inherits MonoBehaviour
    private AppStarter _appStarter;

    // enable/disable features at the beginning of the MainMenu phase
    private IMainMenuStateManager[] _mainMenuManagers;

    // enable/disable features at the beginning of the GamePlay phase
    private IGamePlayStateManager[] _gamePlayManagers;


    /// <summary>
    /// constructor
    /// </summary>
    public AppManager(AppStarter starter, IMainMenuStateManager[] mainMenuManagers, IGamePlayStateManager[] gamePlayManagers)
    {
        // set references
        _appStarter = starter;
        _mainMenuManagers = mainMenuManagers;
        _gamePlayManagers = gamePlayManagers;
        
        // initialize main system
        StartApp();
    }

    /// <summary>
    /// initialize main system of the app
    /// </summary>
    private void StartApp()
    {
        DebugLog.Info(this.ToString(), "the app is started");

        // enable MainMenu phase features
        foreach(IMainMenuStateManager manager in _mainMenuManagers)
        {
            manager.EnableMainMenu();
        }
    }

    /// <summary>
    /// subscribe events to notify state of the phase of the app
    /// </summary>
    /// <param name="appStateSetter"></param>
    public void SubscribeAppStateEvent(IAppStateSetter appStateSetter)
    {
        // set callback
        appStateSetter._onAppStateChange += ChangeAppState;
    }

    /// <summary>
    /// subscribe events to notify state of the gameplay
    /// </summary>
    /// <param name="gamePlayStateSetter"></param>
    public void SubscribeGameplayStateEvent(IGamePlayStateSetter gamePlayStateSetter)
    {
        // set callback
        gamePlayStateSetter._onGamePlayStateChange += ChangeGamePlayState;
    }

    /// <summary>
    /// unsubscribe events to notify state of the phase of the app
    /// </summary>
    /// <param name="appStateSetter"></param>
    public void UnsubscribeAppStateEvent(IAppStateSetter appStateSetter)
    {
        // remove callback
        appStateSetter._onAppStateChange -= ChangeAppState;
    }

    /// <summary>
    /// unsubscribe events to notify state of the gameplay
    /// </summary>
    /// <param name="gamePlayStateSetter"></param>
    public void UnsubscribeGameplayStateEvent(IGamePlayStateSetter gamePlayStateSetter)
    {
        // remove callback
        gamePlayStateSetter._onGamePlayStateChange -= ChangeGamePlayState;
    }

    /// <summary>
    /// change app state
    /// </summary>
    /// <param name="state"></param>
    private void ChangeAppState(object sender, AppStateEventArgs args)
    {
        // change _currentGamePlayState if _currentAppState changes
        if (_currentAppState == AppState.MainMenu && args.appState == AppState.GamePlay)
        {
            // start playing gameplay
            _currentGamePlayState = GamePlayState.Playing;
        }
        else if(_currentAppState == AppState.GamePlay && args.appState == AppState.MainMenu)
        {
            // stop playing gameplay
            _currentGamePlayState = GamePlayState.None;
        }
        
        // update current app state
        _currentAppState = args.appState;

        // notify current app state
        NotifyAppState(_currentAppState);

        DebugLog.Info(this.ToString(), "_currentAppState: " + _currentAppState + " / _currentGamePlayState :" + _currentGamePlayState);
    }

    /// <summary>
    /// change gameplay state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ChangeGamePlayState(object sender, GamePlayStateEventArgs args)
    {
        // record previous gameplay state
        GamePlayState prevState = _currentGamePlayState;

        // set _currentAppState MainMenu if _currentGamePlayState turns None
        if (_currentGamePlayState != GamePlayState.None && args.gamePlayState == GamePlayState.None)
        {
            _currentAppState = AppState.MainMenu;
        }
        
        // update current gameplay state
        _currentGamePlayState = args.gamePlayState;

        // notify current gameplay state
        NotifyGamePlayState(_currentGamePlayState, prevState);

        DebugLog.Info(this.ToString(), "_currentAppState: " + _currentAppState + " / _currentGamePlayState :" + _currentGamePlayState);
    }

    /// <summary>
    /// quit the app
    /// </summary>
    private void QuitApp()
    {
        _appStarter.QuitApp();
    }

    /// <summary>
    /// notify app state to IMainMenuStateManager
    /// </summary>
    private void NotifyAppState(AppState updatedState)
    {
        switch (updatedState)
        {
            case AppState.MainMenu:
                // enable MainMenu phase features
                foreach (IMainMenuStateManager manager in _mainMenuManagers)
                {
                    manager.EnableMainMenu();
                }
                break;

            case AppState.GamePlay:
                // disable MainMenu phase features
                foreach (IMainMenuStateManager manager in _mainMenuManagers)
                {
                    manager.DisableMainMenu();
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// notify gameplay state to IGamePlayStateManager
    /// </summary>
    private void NotifyGamePlayState(GamePlayState updatedState , GamePlayState lastState)
    {
        switch (updatedState)
        {
            case GamePlayState.None:
                // disable gameplay phase features
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    manager.DisableGamePlay();
                }
                break;

            case GamePlayState.Playing:
                // enable gameplay phase features
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    if(lastState == GamePlayState.None)
                    {
                        // if lastState is None (= AppState was MainMenu), enable gameplay features
                        manager.EnableGamePlay();
                    }
                    else if(lastState == GamePlayState.Pause)
                    {
                        // if lastState is Pausing, resume gameplay
                        manager.ResumeGamePlay();
                    }
                }
                break;

            case GamePlayState.Pause:
                // pause gameplay 
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    manager.PauseGamePlay();
                }
                break;

            default:
                break;
        }
    }
}
