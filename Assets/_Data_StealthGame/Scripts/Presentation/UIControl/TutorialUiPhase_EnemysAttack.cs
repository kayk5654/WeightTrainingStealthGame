using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// enemy's attack phase of the tutorial
/// </summary>
public class TutorialUiPhase_EnemysAttack : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("next button")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("color setter for next button")]
    private ButtonBackplateColorSetter _nextButtonColorSetter;

    [SerializeField, Tooltip("warning icon")]
    private WarningIcon _warningIcon;

    [SerializeField, Tooltip("camera transform")]
    private Transform _cameraTransform;

    [SerializeField, Tooltip("node")]
    private Node _node;

    [SerializeField, Tooltip("animator of enemy")]
    private Animator _enemyAnimator;

    // control enemy's animation
    private string _enemyAttackProperty = "IsAttack";

    // distance from center of the warning icon's display area
    private float _distFromCenter = 0.08f;

    // dummy node id for the warning icon
    private int _dummyNodeId = 1000;

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
        // set next button status
        _nextButton.IsEnabled = false;
        _nextButtonColorSetter.SetColor(false);
        
        // set warning icon status
        _warningIcon.SetCameraTransform(_cameraTransform);
        _warningIcon.SetRelativeNodeTransform(_node.transform);
        _warningIcon.SetDistFromCenter(_distFromCenter);
        _warningIcon.SetRelativeNodeId(_dummyNodeId);
        _warningIcon.gameObject.SetActive(true);

        // set enemy status
        _enemyAnimator.SetBool(_enemyAttackProperty, true);

        // set node status
        //_node.Damage(-Config._enemyAttack);
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

        // apply damage of node
        //_node.Damage(Config._enemyAttack);

        // wait for certain period of time
        yield return new WaitForSeconds(4f);

        // enable next button
        _nextButtonColorSetter.SetColor(true);
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }
}
