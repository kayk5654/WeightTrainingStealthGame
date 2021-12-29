using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// phase of difficulty selection
/// </summary>
public class StartGameUiPhase_Difficulty : StartGameUiPhase
{
    [SerializeField, Tooltip("button highlight")]
    private GameObject[] _buttonHighlights;

    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;

    /// <summary>
    /// process to execute displaying this phase
    /// </summary>
    public override void Display()
    {
        base.Display();
        InitializeObjects();
    }

    /// <summary>
    /// initialize object
    /// </summary>
    private void InitializeObjects()
    {
        // hide button highlight
        foreach(GameObject highlight in _buttonHighlights)
        {
            highlight.SetActive(false);
        }

        // disable next button
        _nextButton.IsEnabled = false;
        _nextButtonColorSetter.SetColor(false);
    }

    /// <summary>
    /// enable next button
    /// </summary>
    public void EnableNextButton()
    {
        _nextButton.IsEnabled = true;
        _nextButtonColorSetter.SetColor(true);
    }
}
