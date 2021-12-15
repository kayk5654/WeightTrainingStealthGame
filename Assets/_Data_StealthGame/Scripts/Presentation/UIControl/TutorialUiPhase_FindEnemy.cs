using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// finding enemy phase of the tutorial
/// </summary>
public class TutorialUiPhase_FindEnemy : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;

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
        _tutorialActionHandler.DisplayEnemy(true);
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
        // activate input
        _tutorialActionHandler.EnableAction();

        // wait until the attack action is detected
        yield return new WaitUntil(() => _tutorialActionHandler.IsEnemyFound());

        // enable next button
        _nextButtonColorSetter.SetColor(true);
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }
}
