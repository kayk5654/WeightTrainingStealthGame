using UnityEngine.UI;
using UnityEngine;
using TMPro;
/// <summary>
/// display credits on the ui panel
/// </summary>
public class CreditTextHandler : MonoBehaviour
{
    [SerializeField, Tooltip("text mesh pro")]
    private TextMeshPro _textMeshPro;

    [SerializeField, Tooltip("text box")]
    private Text _text;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        DisplayCredits();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// display credits on the text mesh pro
    /// </summary>
    private void DisplayCredits()
    {
        TextReader textReader = new TextReader();
        string creditText = textReader.ReadData(Config._creditsTextPath);

        if (_textMeshPro)
        {
            _textMeshPro.text = creditText;
        }

        if (_text)
        {
            _text.text = creditText;
        }
    }
}
