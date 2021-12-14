using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// player's action phase of the tutorial
/// </summary>
public class TutorialUiPhase_PlayersAction : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

    // flow of describing player's action
    private IEnumerator _mainSequence;

    // whether the attack action is detected once in this phase
    private bool _isAttackedOnce;

    public override void Display()
    {
        base.Display();
        InitializeObjects();
        InitializeSequence();
    }

    public override void Hide()
    {
        RemoveCallback();
        base.Hide();
    }

    /// <summary>
    /// initialize objects for this phase
    /// </summary>
    private void InitializeObjects()
    {
        _nextButton.IsEnabled = false;
        _isAttackedOnce = false;
        _tutorialActionHandler._tutorialActionCallback += DetectAttackAction;
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
    /// remove callback to detect attack action
    /// </summary>
    private void RemoveCallback()
    {
        _tutorialActionHandler._tutorialActionCallback -= DetectAttackAction;
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
        yield return new WaitUntil(() => _isAttackedOnce);

        // enable next button
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }

    /// <summary>
    /// detect attack action and record it
    /// </summary>
    private void DetectAttackAction()
    {
        _isAttackedOnce = true;
    }
}
