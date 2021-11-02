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

    // control ui features
    UiManager _uiManager;

    // control gameplay features
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
            _isPausing = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _uiManager.SetAppState(AppState.GamePlay);
            _isPausing = false;
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
        _appManager.UnsubscribeAppStateEvent(_uiManager);
        _appManager.UnsubscribeGameplayStateEvent(_uiManager);
        _appManager.UnsubscribeGameplayStateEvent(_gamePlayManager);
    }

    /// <summary>
    /// initialize app logic
    /// </summary>
    private void StartApp()
    {
        // create UiManager
        _uiManager = InitializeUiSystems();

        // create GamePlayManager
        _gamePlayManager = IniitalizeGamePlaySystems();

        // set up AppManager
        InitializeAppMainSystems(_uiManager, _gamePlayManager);
    }

    /// <summary>
    /// initialize app main classes
    /// </summary>
    private void InitializeAppMainSystems(UiManager uiManager, GamePlayManager gamePlayManager)
    {
        // prepare reference for AppManager
        IMainMenuStateManager[] mainMenuStateManagers = { uiManager };
        IGamePlayStateManager[] gamePlayStateManagers = { uiManager, gamePlayManager };

        // create AppManager
        _appManager = new AppManager(this, mainMenuStateManagers, gamePlayStateManagers);

        // set callbacks from UiManager and GamePlayManager to AppManager
        _appManager.SubscribeAppStateEvent(uiManager);
        _appManager.SubscribeGameplayStateEvent(uiManager);
        _appManager.SubscribeGameplayStateEvent(gamePlayManager);
    }

    /// <summary>
    /// initialize features for gameplay 
    /// </summary>
    private GamePlayManager IniitalizeGamePlaySystems()
    {
        // create gameplay manager
        GamePlayManager gamePlayManager = new GamePlayManager();
        
        // create instances of classes to control gameplay features
        LevelManager levelManager = new LevelManager(gamePlayManager);
        PlayerActionManager playerActionManager = new PlayerActionManager(gamePlayManager);

        return gamePlayManager;
    }

    /// <summary>
    /// initialize features for ui control
    /// </summary>
    private UiManager InitializeUiSystems()
    {
        // create instances of classes to control each ui features
        MainUiController mainUiPanelController = new MainUiController();
        WorkoutNavigationUiController workoutNavigationUiController = new WorkoutNavigationUiController();
        OptionMenuUiController optionMenuUiController = new OptionMenuUiController();
        CursorManager cursorManager = FindObjectOfType<CursorManager>();

        // link ui objects in the scene
        UiLinkerProvider uiLinkerProvider = new UiLinkerProvider();
        uiLinkerProvider.LinkObject(mainUiPanelController);
        uiLinkerProvider.LinkObject(workoutNavigationUiController);
        uiLinkerProvider.LinkObject(optionMenuUiController);

        // create ui manager
        UiManager uiManager = new UiManager(mainUiPanelController, workoutNavigationUiController, optionMenuUiController, cursorManager);
        return uiManager;
    }

    /// <summary>
    /// quit this app
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
}
