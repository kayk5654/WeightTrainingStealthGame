using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage ui features
/// </summary>
public class UiManager : IMainMenuStateManager, IGamePlayStateManager, IAppStateSetter
{
    // event to notify the start of MainMenu phase
    public event EventHandler<AppStateEventArgs> _onAppStateChange;

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
    }

    /// <summary>
    /// enable features at the beginning of the MainMenu phase
    /// </summary>
    public void EnableMainMenu()
    {

    }

    /// <summary>
    /// disable features at the beginning of the MainMenu phase
    /// </summary>
    public void DisableMainMenu()
    {

    }

    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    public void EnableGamePlay()
    {

    }

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    public void DisableGamePlay()
    {

    }

    /// <summary>
    /// pause gameplay
    /// </summary>
    public void PauseGamePlay()
    {

    }

    /// <summary>
    /// resume gameplay
    /// </summary>
    public void ResumeGamePlay()
    {

    }

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    public void SetAppState(AppState appState)
    {
        AppStateEventArgs args = new AppStateEventArgs(appState);
        _onAppStateChange.Invoke(this, args);
    }
}
