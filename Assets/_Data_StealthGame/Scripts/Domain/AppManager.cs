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


    /// <summary>
    /// constructor
    /// </summary>
    public AppManager(AppStarter starter)
    {
        _appStarter = starter;
        StartApp();
    }

    /// <summary>
    /// initialize main system of the app
    /// </summary>
    private void StartApp()
    {
        DebugLog.Info(this.ToString(), "the app is started");
    }

    /// <summary>
    /// change app state
    /// </summary>
    /// <param name="state"></param>
    private void ChangeAppState(AppState state)
    {
        // change _currentGamePlayState if _currentAppState changes
        if (_currentAppState == AppState.MainMenu && state == AppState.GamePlay)
        {
            // start playing gameplay
            _currentGamePlayState = GamePlayState.Playing;
        }
        else if(_currentAppState == AppState.GamePlay && state == AppState.MainMenu)
        {
            // stop playing gameplay
            _currentGamePlayState = GamePlayState.None;
        }
        
        _currentAppState = state;

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
