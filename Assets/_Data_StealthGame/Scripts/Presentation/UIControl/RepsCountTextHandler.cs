using TMPro;
using UnityEngine;
/// <summary>
/// display number of the selected exercise the player was able to do
/// </summary>
public class RepsCountTextHandler : TranslatedText
{
    [SerializeField, Tooltip("get reps in the gameplay")]
    private RepsCounter _repsCounter;

    [SerializeField, Tooltip("textmesh to display number")]
    private TextMeshPro _numberText;


    /// <summary>
    /// display reps when this gameobject is enabled
    /// </summary>
    private void OnEnable()
    {
        DisplayReps();
    }

    protected override void Update()
    {
        
    }

    /// <summary>
    /// set text to display
    /// </summary>
    /// <param name="text"></param>
    public override void SetText(string text)
    {
        base.SetText(text);
    }

    /// <summary>
    /// display nubmer of the selected exercise the player was able to do
    /// </summary>
    private void DisplayReps()
    {
        _numberText.text = _repsCounter.GetReps().ToString();
    }
}
