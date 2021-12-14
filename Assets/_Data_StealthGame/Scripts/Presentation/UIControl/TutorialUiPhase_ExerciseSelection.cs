using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class TutorialUiPhase_ExerciseSelection : TutorialUiPhase
{
    [SerializeField, Tooltip("button to go to the next phase")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("toggle plate of the squat button")]
    private GameObject _squatTogglePlate;


    /// <summary>
    /// initialize when this phase is activated
    /// </summary>
    private void OnEnable()
    {
        InitButtonState();
    }

    /// <summary>
    /// initialize button status
    /// </summary>
    private void InitButtonState()
    {
        _nextButton.IsEnabled = false;
        _squatTogglePlate.SetActive(false);
    }
}
