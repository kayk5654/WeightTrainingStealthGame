using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// attacking enemy phase of the tutorial
/// </summary>
public class TutorialUiPhase_AtatckEnemy : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

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
        yield return new WaitUntil(() => _tutorialActionHandler.IsEnemyDestroyed());

        // disable input action
        _tutorialActionHandler.DisableAction();

        // enable next button
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }
}
