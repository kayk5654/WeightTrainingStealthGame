﻿using System.Collections;
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
        foreach(IUiPhase phase in _uiPhases.Values)
        {
            phase._onMoveToSelectedPhase -= MoveToSelectedPhase;
        }
    }

    /// <summary>
    /// contain ui phase in the dictionary
    /// </summary>
    /// <param name="phase"></param>
    public void SetUiPhase(IUiPhase phase)
    {
        // check whether the phase id of the phase is valid
        if (phase.GetPhaseId() >= (int)TutorialPhase.LENGTH || phase.GetPhaseId() < 0) { return; }

        // add phase to the dictionary
        _uiPhases.Add((TutorialPhase)phase.GetPhaseId(), phase);

        // set callback of the button on the phase
        phase._onMoveToSelectedPhase += MoveToSelectedPhase;
    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {
        // if the selected ui phase is none, hide all panels
        if (phaseIndex == (int)TutorialPhase.None)
        {
            foreach (TutorialPhase key in _uiPhases.Keys)
            {
                _uiPhases[key].Hide();
            }
            return;
        }

        // check whether the given phaseIndex is valid
        if (phaseIndex >= (int)TutorialPhase.LENGTH || phaseIndex < 0) { return; }


        foreach (TutorialPhase key in _uiPhases.Keys)
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
    /// switch  ui phase to display depending on the button input from any ui phases under this class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveToSelectedPhase(object sender, UiPhaseEventArgs args)
    {
        DisplayUiPhase(args._selectedPhaseId);
    }
}
