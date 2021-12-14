using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// initial posture calibration phase of the tutorial
/// </summary>
public class TutorialUiPhase_InitialPosture : TutorialUiPhase
{
    [SerializeField, Tooltip("tutorial action control")]
    private TutorialActionHandler _tutorialActionHandler;

    [SerializeField, Tooltip("stay still time checker")]
    private StayStillTimeChecker _stayStillTimeChecker;

    [SerializeField, Tooltip("button to go to the next phase")]
    private Interactable _nextButton;

    [SerializeField, Tooltip("control ring gage")]
    private RingGageHandler _ringGageHandler;

    // flow of posture calibration
    private IEnumerator _mainSequence;


    /// <summary>
    /// initialize when this phase is activated
    /// </summary>
    public override void Display()
    {
        base.Display();
        InitializeObjects();
        InitializeSequence();
    }

    /// <summary>
    /// initialize button status
    /// </summary>
    private void InitializeObjects()
    {
        _nextButton.IsEnabled = false;
        _ringGageHandler.enabled = true;
    }

    /// <summary>
    /// initialize posture calibration process
    /// </summary>
    private void InitializeSequence()
    {
        if(_mainSequence != null)
        {
            _mainSequence = null;
        }
        
        _mainSequence = MainSequence();
        StartCoroutine(_mainSequence);
    }

    /// <summary>
    /// process of posture calibration
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainSequence()
    {
        float interval = 2f;
        yield return new WaitForSeconds(interval);

        // initialize time counting
        _stayStillTimeChecker.ResetTimeCount();

        // enable ring gage update
        _ringGageHandler._enableUpdating = true;
        
        // start time counting
        _stayStillTimeChecker.StartTimeCount();

        // wait until the ring gage is filled
        yield return new WaitUntil(() => _stayStillTimeChecker.GetNormalizedTime() == 1f);

        // initialize exercise input
        _tutorialActionHandler.InitAction();

        // disable ring gage update
        _ringGageHandler.enabled = false;

        // enable next button
        _nextButton.IsEnabled = true;
        _mainSequence = null;
    }
}
