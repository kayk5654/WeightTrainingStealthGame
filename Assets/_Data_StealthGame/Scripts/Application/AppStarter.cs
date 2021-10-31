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

    // callbacks from UiManager and GamePlayManager to AppManager
    private EventHandler<AppStateEventArgs>[] _appStateCallbacks;

    /// <summary>
    /// initialization on MonoBehaviour
    /// </summary>
    private void Start()
    {
        StartApp();
    }

    /// <summary>
    /// execute shut down process
    /// </summary>
    private void OnDestroy()
    {
        _appManager.UnsubscribeEvent(_appStateCallbacks);
    }

    /// <summary>
    /// initialize app logic
    /// </summary>
    private void StartApp()
    {
        // create UiManager
        UiManager uiManager = new UiManager();

        // create GamePlayManager
        GamePlayManager gamePlayManager = new GamePlayManager();

        // prepare reference for AppManager
        IMainMenuStateManager[] mainMenuStateManagers = { uiManager };
        IGamePlayStateManager[] gamePlayStateManagers = { uiManager, gamePlayManager };

        // create AppManager
        _appManager = new AppManager(this, mainMenuStateManagers, gamePlayStateManagers);

        // set callbacks from UiManager and GamePlayManager to AppManager
        _appStateCallbacks = new EventHandler<AppStateEventArgs>[]
        {
            uiManager._onStartGamePlayState,
            uiManager._onStartMainMenuState,
            gamePlayManager._onStartGamePlayState
        };

        _appManager.SubscribeEvent(_appStateCallbacks);

    }

    /// <summary>
    /// quit this app
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
}
