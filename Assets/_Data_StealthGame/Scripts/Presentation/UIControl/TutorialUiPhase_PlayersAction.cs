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

    [SerializeField, Tooltip("object for squat down step")]
    private GameObject _squatDownObject;

    [SerializeField, Tooltip("object for lock on step")]
    private GameObject _lockOnObject;

    [SerializeField, Tooltip("object for stand up step")]
    private GameObject _standUpObject;

    [SerializeField, Tooltip("detect whether any objects are locked on")]
    private ProjectileTargetFinder _projectileTargetFinder;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;

    // flow of describing player's action
    private IEnumerator _mainSequence;

    // whether the attack action is detected once in this phase
    private bool _isAttackedOnce;

    // whether the negative movement is detected once in this phase
    private bool _isSquatDownOnce;

    public override void Display()
    {
        base.Display();
        InitializeObjects();
        InitializeSequence();

        // if the previous phase is FindEnemy, destroy sample enemy
        _tutorialActionHandler.DisplayEnemy(false);
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
        _isSquatDownOnce = false;
        _tutorialActionHandler._pushCallback += DetectAttackAction;
        _tutorialActionHandler._startHoldCallback += DetectSquatDownAction;
        _squatDownObject.SetActive(true);
        _standUpObject.SetActive(false);
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
    /// remove callback to detect attack action
    /// </summary>
    private void RemoveCallback()
    {
        _tutorialActionHandler._pushCallback -= DetectAttackAction;
        _tutorialActionHandler._startHoldCallback -= DetectSquatDownAction;
    }

    /// <summary>
    /// process of describing player's action
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainSequence()
    {
        // activate input
        _tutorialActionHandler.EnableAction();

        // wait until the squat down action is detected
        yield return new WaitUntil(() => _isSquatDownOnce);

        // switch text
        _squatDownObject.SetActive(false);
        _lockOnObject.SetActive(true);

        // wait until any objects are locked on
        yield return new WaitUntil(() => _projectileTargetFinder.GetTargetObject() != null);

        // switch text
        _lockOnObject.SetActive(false);
        _standUpObject.SetActive(true);

        // wait until the attack action is detected
        yield return new WaitUntil(() => _isAttackedOnce);

        // enable next button
        _nextButtonColorSetter.SetColor(true);
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

    /// <summary>
    /// detect squat down movement and record it
    /// </summary>
    private void DetectSquatDownAction()
    {
        _isSquatDownOnce = true;
    }
}
