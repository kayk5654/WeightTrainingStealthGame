using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class TutorialUiPhase_ExerciseSelection : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("button to go to the next phase")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("toggle plate of the squat button")]
    private GameObject _squatTogglePlate;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;


    /// <summary>
    /// initialize when this phase is activated
    /// </summary>
    public override void Display()
    {
        base.Display();
        InitButtonState();
    }

    /// <summary>
    /// initialize button status
    /// </summary>
    private void InitButtonState()
    {
        _nextButton.IsEnabled = false;
        _squatTogglePlate.SetActive(false);
        _nextButtonColorSetter.SetColor(false);
    }

    /// <summary>
    /// initialize exercise input with the squat setting
    /// </summary>
    public void SelectSquat()
    {
        _tutorialActionHandler.SelectSquat();
        _nextButtonColorSetter.SetColor(true);
    }
}
