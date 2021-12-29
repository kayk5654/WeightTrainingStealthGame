using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
/// <summary>
/// disable buttons in the same ui panel
/// </summary>
public class MRTKButtonDisabler : MonoBehaviour, IButtonFeedback
{
    [SerializeField, Tooltip("buttons in the same ui panel")]
    private Interactable[] _interactables;

    [SerializeField, Tooltip("this button interaction")]
    private Interactable _thisInteractable;

    [SerializeField, Tooltip("default enable/disable state of this button's interaction")]
    private bool _thisInteractableDefaultState = true;

    /// <summary>
    /// when the GameObject of this button is active, enable button input
    /// </summary>
    private void OnEnable()
    {
        _thisInteractable.IsEnabled = _thisInteractableDefaultState;
    }

    /// <summary>
    /// get duration of feedback animation
    /// if therre's no animations, return 0
    /// </summary>
    /// <returns></returns>
    public float GetFeebackDuration(ButtonFeedbackType feedbackType)
    {
        return 0f;
    }

    /// <summary>
    /// disable relative button interaction when this button is pressed
    /// </summary>
    public void ExecuteFeedback(ButtonFeedbackType feedbackType)
    {
        if(feedbackType != ButtonFeedbackType.OnPressed) { return; }

        foreach(Interactable interactable in _interactables)
        {
            interactable.IsEnabled = false;
        }
    }
}
