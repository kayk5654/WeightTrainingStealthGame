using System.Collections;
using System.Collections.Generic;
/// <summary>
/// names of phases in MainUiPanelController
/// </summary>
[System.Serializable]
public enum MainUiPanelPhase
{
    None = -1,
    Root = 0,
    Tutorial = 1,
    SelectExercise = 2,
    Settings = 3,
    Quit = 4,
    LENGTH,
}
/// <summary>
/// control main menu ui
/// </summary>
public class MainUiController : IMultiPhaseUi
{
    // ui phases to control
    private Dictionary<MainUiPanelPhase, IUiPhase> _uiPhases;


    /// <summary>
    /// constructor
    /// </summary>
    public MainUiController()
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
        // if the selected ui phase is none, hide all panels
        if(phaseIndex == (int)MainUiPanelPhase.None)
        {
            foreach (MainUiPanelPhase key in _uiPhases.Keys)
            {
                _uiPhases[key].Hide();
            }
            return;
        }
        
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
