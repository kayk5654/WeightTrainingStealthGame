using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// root phase of the main gameplay under MainUiController
/// </summary>
public class MainUiPhase_StartGame : MainUiPhase
{
    [SerializeField, Tooltip("all start game phase in the scene")]
    private StartGameUiPhase[] _playGamePhases;


    /// <summary>
    /// revert tutorial phase objects initial condition
    /// </summary>
    public override void Hide()
    {
        base.Hide();

        foreach (StartGameUiPhase phase in _playGamePhases)
        {
            if (phase.GetPhaseId() == (int)StartGamePhase.SelectDifficulty)
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
