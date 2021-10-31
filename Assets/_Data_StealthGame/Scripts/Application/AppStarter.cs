using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// initialize settings and classes on starting this app
/// </summary>
public class AppStarter : MonoBehaviour
{
    // main class to manage this app
    AppManager _appManager;

    UiManager _uiManager;
    GamePlayManager _gamePlayManager;


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
        /*
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _uiManager.SetAppState(AppState.MainMenu);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _uiManager.SetAppState(AppState.GamePlay);
        }
        */
    }

    /// <summary>
    /// execute shut down process
    /// </summary>
    private void OnDestroy()
    {
        _appManager.UnsubscribeEvent(_uiManager as IAppStateSetter);
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
        _appManager.SubscribeEvent(_uiManager as IAppStateSetter);

    }

    /// <summary>
    /// quit this app
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
}
