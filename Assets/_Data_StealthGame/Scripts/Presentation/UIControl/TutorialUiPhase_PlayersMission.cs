using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// player's mission phase of the tutorial
/// </summary>
public class TutorialUiPhase_PlayersMission : TutorialUiPhase
{
    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;

    // duration of animation
    private float _baseAnimationDuration = 4f;

    // flow of describing player's action
    private IEnumerator _mainSequence;

    public override void Display()
    {
        base.Display();
        InitializeObjects();
        InitializeSequence();
    }

    public override void Hide()
    {

        base.Hide();
    }

    /// <summary>
    /// initialize objects for this phase
    /// </summary>
    private void InitializeObjects()
    {
        _nextButton.IsEnabled = false;
        _nextButtonColorSetter.SetColor(false);
    }

    /// <summary>
    /// initialize describing player's action process
    /// </summary>
    private void InitializeSequence()
    {
        if (_mainSequence != null)
        {
            _mainSequence = null;
        }

        _mainSequence = MainSequence();
        StartCoroutine(_mainSequence);
    }

    /// <summary>
    /// process of describing player's action
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainSequence()
    {
        // wait until the first loop of the animation ends
        yield return new WaitForSeconds(_baseAnimationDuration);

        // enable next button
        _nextButtonColorSetter.SetColor(true);
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }
}
