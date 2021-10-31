using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// initialize settings and classes on starting this app
/// </summary>
public class AppStarter : MonoBehaviour
{
    // main class to manage this app
    AppManager _appManager;

    // manages ui depending on the app and gameplay phases
    UiManager _uiManager;

    // manages gameplay objects depending on the gameplay phases
    GamePlayManager _gamePlayManager;

    // debugging
    private bool _isPausing;

    /// <summary>
    /// initialization on MonoBehaviour
    /// </summary>
    private void Start()
    {
        StartApp();
    }
    
    /// <summary>
    /// debugging
    /// </summary>
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _uiManager.SetAppState(AppState.MainMenu);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _uiManager.SetAppState(AppState.GamePlay);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_isPausing)
            {
                _gamePlayManager.SetGamePlayState(GamePlayState.Playing);
            }
            else
            {
                _gamePlayManager.SetGamePlayState(GamePlayState.Pausing);
            }
            _isPausing = !_isPausing;
        }
        
    }

    /// <summary>
    /// execute shut down process
    /// </summary>
    private void OnDestroy()
    {
        _appManager.UnsubscribeEvent(_uiManager as IAppStateSetter, _gamePlayManager as IGamePlayStateSetter);
    }

    /// <summary>
    /// initialize app logic
    /// </summary>
    private void StartApp()
    {
        // create UiManager
        _uiManager = new UiManager();

        // create GamePlayManager
        _gamePlayManager = new GamePlayManager();

        // prepare reference for AppManager
        IMainMenuStateManager[] mainMenuStateManagers = { _uiManager };
        IGamePlayStateManager[] gamePlayStateManagers = { _uiManager, _gamePlayManager };

        // create AppManager
        _appManager = new AppManager(this, mainMenuStateManagers, gamePlayStateManagers);

        // set callbacks from UiManager and GamePlayManager to AppManager
        _appManager.SubscribeEvent(_uiManager as IAppStateSetter, _gamePlayManager as IGamePlayStateSetter);

    }

    /// <summary>
    /// quit this app
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
}
