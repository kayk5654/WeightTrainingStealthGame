using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control tutorial phase
/// </summary>
public class TutorialUiController : IMultiPhaseUi
{
    // ui phases to control
    private Dictionary<TutorialPhase, IUiPhase> _uiPhases;


    /// <summary>
    /// constructor
    /// </summary>
    public TutorialUiController()
    {
        _uiPhases = new Dictionary<TutorialPhase, IUiPhase>();
    }

    /// <summary>
    /// destructor
    /// </summary>
    ~TutorialUiController()
    {

    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {

    }
}
