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
    /// subscribe events to notify start of the phase of the app
    /// </summary>
    /// <param name="eventHandlers"></param>
    public void SubscribeEvent(IAppStateSetter appStateSetter)
    {
        // set callback
        appStateSetter._onAppStateChange += ChangeAppState;
    }

    /// <summary>
    /// unsubscribe events to notify start of the phase of the app
    /// </summary>
    /// <param name="eventHandlers"></param>
    public void UnsubscribeEvent(IAppStateSetter appStateSetter)
    {
        // remove callback
        appStateSetter._onAppStateChange -= ChangeAppState;
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

        DebugLog.Info(this.ToString(), "_currentAppState: " + _currentAppState + " / _currentGamePlayState :" + _currentGamePlayState);
    }

    /// <summary>
    /// quit the app
    /// </summary>
    private void QuitApp()
    {
        _appStarter.QuitApp();
    }
}
