using System.Collections;
using System.Collections.Generic;
/// <summary>
/// control ui for workout navigation during gameplay
/// </summary>
public class WorkoutNavigationUiController : IMultiPhaseUi
{
    // ui phases to control
    private Dictionary<MainUiPanelPhase, IUiPhase> _uiPhases;


    /// <summary>
    /// constructor
    /// </summary>

    public WorkoutNavigationUiController()
    {
        _uiPhases = new Dictionary<MainUiPanelPhase, IUiPhase>();
    }

    /// <summary>
    /// contain ui phase in the dictionary
    /// </summary>
    /// <param name="phase"></param>
    public void SetUiPhase(IUiPhase phase)
    {
        // check whether the phase id of the phase is valid
        if (phase.GetPhaseId() >= (int)MainUiPanelPhase.LENGTH || phase.GetPhaseId() < 0) { return; }

        _uiPhases.Add((MainUiPanelPhase)phase.GetPhaseId(), phase);

        DebugLog.Info(this.ToString(), "ui phase added / " + _uiPhases.Count);
    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {
        foreach (MainUiPanelPhase key in _uiPhases.Keys)
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
