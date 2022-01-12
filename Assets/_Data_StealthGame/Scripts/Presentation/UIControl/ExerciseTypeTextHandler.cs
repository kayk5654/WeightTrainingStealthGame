using UnityEngine;
/// <summary>
/// display selected exercise type
/// </summary>
public class ExerciseTypeTextHandler : TranslatedText
{
    [SerializeField, Tooltip("get text translation data")]
    private TextTranslationManager _textTranslationManager;


    /// <summary>
    /// display reps when this gameobject is enabled
    /// </summary>
    private void OnEnable()
    {
        DisplayExerciseType();
    }

    protected override void Update()
    {

    }

    /// <summary>
    /// display selected exercise type
    /// </summary>
    private void DisplayExerciseType()
    {
        base.SetText(_textTranslationManager.GetTextTranslationData(_id));
    }

    /// <summary>
    /// set exercise type
    /// </summary>
    public void SetExerciseTypeTextNum(int textNum)
    {
        _id = textNum;
    }

}
