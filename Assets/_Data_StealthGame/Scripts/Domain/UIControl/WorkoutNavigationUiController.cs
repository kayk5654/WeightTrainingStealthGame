using System;
using System.Collections.Generic;
/// <summary>
/// control ui for workout navigation during gameplay
/// </summary>
public class WorkoutNavigationUiController : IMultiPhaseUi, IGamePlayStateSetter
{
    // ui phases to control
    private Dictionary<WorkoutNavigationUiPanelPhase, IUiPhase> _uiPhases;

    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // get trigger to start workout
    private IGamePlayStateSetter[] _gamePlayStateSetters;


    /// <summary>
    /// constructor
    /// </summary>

    public WorkoutNavigationUiController()
    {
        _uiPhases = new Dictionary<WorkoutNavigationUiPanelPhase, IUiPhase>();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    ~WorkoutNavigationUiController()
    {
        if(_gamePlayStateSetters == null || _gamePlayStateSetters.Length < 1) { return; }
        foreach(IGamePlayStateSetter setter in _gamePlayStateSetters)
        {
            setter._onGamePlayStateChange -= SendGamePlayState;
        }
    }


    /// <summary>
    /// contain ui phase in the dictionary
    /// </summary>
    /// <param name="phase"></param>
    public void SetUiPhase(IUiPhase phase)
    {
        // check whether the phase id of the phase is valid
        if (phase.GetPhaseId() >= (int)WorkoutNavigationUiPanelPhase.LENGTH || phase.GetPhaseId() < 0) { return; }

        // add phase to the dictionary
        _uiPhases.Add((WorkoutNavigationUiPanelPhase)phase.GetPhaseId(), phase);
    }

    /// <summary>
    /// set reference of IGamePlayStateSetter that triggers actual workout
    /// </summary>
    /// <param name="gamePlayStateSetters"></param>
    public void SetGamePlayStateSetter(IGamePlayStateSetter[] gamePlayStateSetters)
    {
        _gamePlayStateSetters = gamePlayStateSetters;
        foreach (IGamePlayStateSetter setter in _gamePlayStateSetters)
        {
            setter._onGamePlayStateChange += SendGamePlayState;
        }
    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {
        // if the selected ui phase is none, hide all panels
        if (phaseIndex == (int)WorkoutNavigationUiPanelPhase.None)
        {
            foreach (WorkoutNavigationUiPanelPhase key in _uiPhases.Keys)
            {
                _uiPhases[key].Hide();
            }
            return;
        }

        // check whether the given phaseIndex is valid
        if (phaseIndex >= (int)WorkoutNavigationUiPanelPhase.LENGTH || phaseIndex < 0) { return; }


        foreach (WorkoutNavigationUiPanelPhase key in _uiPhases.Keys)
        {
            if ((int)key == phaseIndex)
            {
                _uiPhases[key].Display();
            }
            else
            {
                _uiPhases[key].Hide();
            }
        }
    }


    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }

    /// <summary>
    /// send update of the gameplay state to the upper class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void SendGamePlayState(object sender, GamePlayStateEventArgs args)
    {
        _onGamePlayStateChange?.Invoke(sender, args);
    }

}
