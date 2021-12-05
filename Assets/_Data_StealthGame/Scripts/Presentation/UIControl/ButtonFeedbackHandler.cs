using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// control button feedback
/// </summary>
public class ButtonFeedbackHandler : MonoBehaviour
{
    [Tooltip("callback of this button")]
    public UnityEvent _onClicked;

    [SerializeField, Tooltip("set execute timing of _onClicked; it's called when the button is pressed as a default")]
    private bool _callOnClicedOnRelease = false;

    // button feedbacks; feedback must be attached on the same GameObject
    private IButtonFeedback[] _buttonFeedbacks;
    
    
    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _buttonFeedbacks = GetComponents<IButtonFeedback>();
    }

    /// <summary>
    /// feedback when the button is pressed
    /// </summary>
    public void OnPressed()
    {
        StartCoroutine(ButtonFeedbackSequence(ButtonFeedbackType.OnPressed));
    }

    /// <summary>
    /// feedback when the button is released
    /// </summary>
    public void OnReleased()
    {
        StartCoroutine(ButtonFeedbackSequence(ButtonFeedbackType.OnReleased));
    }

    /// <summary>
    /// feedback when the button is pointed
    /// </summary>
    public void OnPointed()
    {
        StartCoroutine(ButtonFeedbackSequence(ButtonFeedbackType.OnPointed));
    }

    /// <summary>
    /// feedback when button pointing is end
    /// </summary>
    public void OnPointedEnd()
    {
        StartCoroutine(ButtonFeedbackSequence(ButtonFeedbackType.OnPointedEnd));
    }

    /// <summary>
    /// process of the feedback
    /// </summary>
    /// <param name="feedbackType"></param>
    /// <returns></returns>
    private IEnumerator ButtonFeedbackSequence(ButtonFeedbackType feedbackType)
    {
        float feedbackDuration = 0;

        foreach(IButtonFeedback feedback in _buttonFeedbacks)
        {
            // check duration of the feedbacks
            float duration = feedback.GetFeebackDuration(feedbackType);
            feedbackDuration = feedbackDuration < duration ? duration : feedbackDuration;

            // execute feedbakc itself
            feedback.ExecuteFeedback(feedbackType);
        }

        // wait until the feedback ends
        yield return new WaitForSeconds(feedbackDuration);

        // execute callback of this button
        if(feedbackType != ButtonFeedbackType.OnPressed && feedbackType != ButtonFeedbackType.OnReleased) { yield break; }
        
        if ((_callOnClicedOnRelease && feedbackType == ButtonFeedbackType.OnReleased) ||
            (!_callOnClicedOnRelease && feedbackType == ButtonFeedbackType.OnPressed))
        {
            _onClicked.Invoke();
        }
    }
}
