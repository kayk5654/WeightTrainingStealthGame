/// <summary>
/// interface of button feedbacks
/// </summary>
public interface IButtonFeedback
{
    /// <summary>
    /// get duration of feedback animation
    /// if therre's no animations, return 0
    /// </summary>
    /// <returns></returns>
    float GetFeebackDuration(ButtonFeedbackType feedbackType);

    /// <summary>
    /// execute button feedback
    /// </summary>
    void ExecuteFeedback(ButtonFeedbackType feedbackType);
}
