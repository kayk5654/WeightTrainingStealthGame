using System.Collections;
using System.Collections.Generic;
/// <summary>
/// control ui for workout navigation during gameplay
/// </summary>
public class WorkoutNavigationUiController : IMultiPhaseUi
{
    // ui phases to control
    private Dictionary<WorkoutNavigationUiPanelPhase, IUiPhase> _uiPhases;


    /// <summary>
    /// constructor
    /// </summary>

    public WorkoutNavigationUiController()
    {
        _uiPhases = new Dictionary<WorkoutNavigationUiPanelPhase, IUiPhase>();
    }

    /// <summary>
    /// contain ui phase in the dictionary
    /// </summary>
    /// <param name="phase"></param>
    public void SetUiPhase(IUiPhase phase)
    {
        // check whether the phase id of the phase is valid
        if (phase.GetPhaseId() >= (int)WorkoutNavigationUiPanelPhase.LENGTH || phase.GetPhaseId() < 0) { return; }

        _uiPhases.Add((WorkoutNavigationUiPanelPhase)phase.GetPhaseId(), phase);

        DebugLog.Info(this.ToString(), "ui phase added / " + _uiPhases.Count);
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
}
