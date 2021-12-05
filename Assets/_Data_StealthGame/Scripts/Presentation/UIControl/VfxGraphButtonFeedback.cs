using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// control button feedback by VFX Graph
/// </summary>
public class VfxGraphButtonFeedback : MonoBehaviour, IButtonFeedback
{

    [SerializeField, Tooltip("vfx graph for OnPressed feedback")]
    private VisualEffect _onPressedVfx;

    [SerializeField, Tooltip("vfx graph for OnPointed feedback")]
    private VisualEffect _onPointedVfx;

    [SerializeField, Tooltip("duration of OnPressed feedback")]
    private float _onPressedDuration;

    [SerializeField, Tooltip("duration of OnReleased feedback")]
    private float _onReleasedDuration;

    [SerializeField, Tooltip("duration of OnPointed feedback")]
    private float _onPointedDuration;

    [SerializeField, Tooltip("duration of OnPointedEnd feedback")]
    private float _onPointedEndDuration;


    /// <summary>
    /// get duration of feedback animation
    /// if therre's no animations, return 0
    /// </summary>
    /// <returns></returns>
    public float GetFeebackDuration(ButtonFeedbackType feedbackType)
    {
        switch (feedbackType)
        {
            case ButtonFeedbackType.OnPressed:
                return _onPressedDuration;

            case ButtonFeedbackType.OnReleased:
                return _onReleasedDuration;

            case ButtonFeedbackType.OnPointed:
                return _onPointedDuration;

            case ButtonFeedbackType.OnPointedEnd:
                return _onPointedEndDuration;

            default:
                return 0f;

        }
    }

    /// <summary>
    /// execute button feedback
    /// </summary>
    public void ExecuteFeedback(ButtonFeedbackType feedbackType)
    {
        switch (feedbackType)
        {
            case ButtonFeedbackType.OnPressed:
                ExecuteOnPressed();
                break;

            case ButtonFeedbackType.OnReleased:
                ExecuteOnReleased();
                break;

            case ButtonFeedbackType.OnPointed:
                ExecuteOnPointed();
                break;

            case ButtonFeedbackType.OnPointedEnd:
                ExecuteOnPointedEnd();
                break;

            default:
                break;

        }
    }

    /// <summary>
    /// execute OnPressed feedback
    /// </summary>
    private void ExecuteOnPressed()
    {
        if (!_onPressedVfx) { return; }
        _onPressedVfx.gameObject.SetActive(true);
        _onPressedVfx.Play(default);
    }

    /// <summary>
    /// execute OnReleased feedback
    /// </summary>
    private void ExecuteOnReleased()
    {
        if (!_onPressedVfx) { return; }
        _onPressedVfx.Stop();
        _onPressedVfx.gameObject.SetActive(false);
    }

    /// <summary>
    /// execute OnPointed feedback
    /// </summary>
    private void ExecuteOnPointed()
    {
        if (!_onPointedVfx) { return; }
        _onPointedVfx.gameObject.SetActive(true);
        _onPointedVfx.Play(default);
    }

    /// <summary>
    /// execute OnPointedEnd feedback
    /// </summary>
    private void ExecuteOnPointedEnd()
    {
        if (!_onPointedVfx) { return; }
        _onPointedVfx.Stop();
        _onPointedVfx.gameObject.SetActive(false);
    }

}
