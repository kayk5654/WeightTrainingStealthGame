using System.Collections;
using System.Collections.Generic;
/// <summary>
/// names of phases in MainUiPanelController
/// </summary>
[System.Serializable]
public enum MainUiPanelPhase
{
    root = 0,
    tutorial = 1,
    selectExercise = 2,
    settings = 3,
    quit = 4,
    LENGTH = 5,
}
/// <summary>
/// control main menu ui
/// </summary>
public class MainUiPanelController : IMultiPhaseUi
{
    // ui phases to control
    private Dictionary<MainUiPanelPhase, IUiPhase> _uiPhases;


    /// <summary>
    /// constructor
    /// </summary>
    public MainUiPanelController()
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
    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {
        // check whether the given phaseIndex is valid
        if (phaseIndex >= (int)MainUiPanelPhase.LENGTH || phaseIndex < 0) { return; }
        
        foreach(MainUiPanelPhase key in _uiPhases.Keys)
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
