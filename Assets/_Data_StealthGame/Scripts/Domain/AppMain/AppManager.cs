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

    private IExerciseInfoSetter _exerciseInforSetter;

    /// <summary>
    /// constructor
    /// </summary>
    public AppManager(AppStarter starter, IMainMenuStateManager[] mainMenuManagers, IGamePlayStateManager[] gamePlayManagers, IExerciseInfoSetter exerciseInforSetter)
    {
        // set references
        _appStarter = starter;
        _mainMenuManagers = mainMenuManagers;
        _gamePlayManagers = gamePlayManagers;
        _exerciseInforSetter = exerciseInforSetter;

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
    /// subscribe events to notify state of the exercise info
    /// </summary>
    /// <param name="exerciseInfoSetter"></param>
    public void SubscribeExerciseInfoEvent(IExerciseInfoSender exerciseInfoSetter)
    {
        // set callback
        exerciseInfoSetter._onExerciseSelected += ChangeExerciseInfo;
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
    /// unsubscribe events to notify state of the exercise info
    /// </summary>
    /// <param name="exerciseInfoSetter"></param>
    public void UnubscribeExerciseInfoEvent(IExerciseInfoSender exerciseInfoSetter)
    {
        // remove callback
        exerciseInfoSetter._onExerciseSelected -= ChangeExerciseInfo;
    }

    /// <summary>
    /// change app state
    /// </summary>
    /// <param name="state"></param>
    private void ChangeAppState(object sender, AppStateEventArgs args)
    {
        // do nothing if AppState isn't change
        if(_currentAppState == args.appState) { return; }
        
        // record previous gameplay state
        GamePlayState prevGameplayState = _currentGamePlayState;

        // change _currentGamePlayState whem _currentAppState changes
        if (_currentAppState == AppState.MainMenu && args.appState == AppState.GamePlay)
        {
            // start playing gameplay
            _currentGamePlayState = GamePlayState.BeforePlay;

            // notify current gameplay state
            NotifyGamePlayState(_currentGamePlayState, prevGameplayState);
        }
        else if(_currentAppState == AppState.GamePlay && args.appState == AppState.MainMenu)
        {
            // stop playing gameplay
            _currentGamePlayState = GamePlayState.None;

            // notify current gameplay state
            NotifyGamePlayState(_currentGamePlayState, prevGameplayState);
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
        // do nothing if GamePlayState isn't change
        if(_currentGamePlayState == args.gamePlayState) { return; }
        
        // set _currentAppState MainMenu if _currentGamePlayState turns None
        if (_currentGamePlayState != GamePlayState.None && args.gamePlayState == GamePlayState.None)
        {
            _currentAppState = AppState.MainMenu;

            // notify current app state
            NotifyAppState(_currentAppState);
        }

        // notify current gameplay state
        NotifyGamePlayState(args.gamePlayState, _currentGamePlayState);

        // update current gameplay state
        _currentGamePlayState = args.gamePlayState;

        DebugLog.Info(this.ToString(), "_currentAppState: " + _currentAppState + " / _currentGamePlayState :" + _currentGamePlayState);
    }

    /// <summary>
    /// change exercise info
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ChangeExerciseInfo(object sender, ExerciseInfoEventArgs args)
    {
        _exerciseInforSetter.ChangeExerciseType(args._selectedExercise);
        DebugLog.Info(this.ToString(), "exercise info :" + args._selectedExercise);
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

            case GamePlayState.BeforePlay:
                // display before gameplay ui, etc.
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    manager.BeforeGamePlay();
                }
                break;

            case GamePlayState.Playing:
                // enable gameplay phase features
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    if(lastState == GamePlayState.BeforePlay)
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

            case GamePlayState.AfterPlay:
                // display game clear or game over dialog
                foreach (IGamePlayStateManager manager in _gamePlayManagers)
                {
                    manager.AfterGamePlay();
                }
                break;

            default:
                break;
        }
    }
}
