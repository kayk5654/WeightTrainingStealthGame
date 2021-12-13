using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// root phase of tutorial under MainUiController
/// </summary>
public class MainUiPhase_Tutorial : MainUiPhase
{
    [SerializeField, Tooltip("all tutorial phase in the scene")]
    private TutorialUiPhase[] _tutorialPhases;


    /// <summary>
    /// revert tutorial phase objects initial condition
    /// </summary>
    public override void Hide()
    {
        base.Hide();

        foreach(TutorialUiPhase phase in _tutorialPhases)
        {
            if(phase.GetPhaseId() == (int)TutorialPhase.Intro)
            {
                phase.Display();
            }
            else
            {
                phase.Hide();
            }
        }
    }
}
