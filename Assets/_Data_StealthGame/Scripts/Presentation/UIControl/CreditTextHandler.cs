using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// display credits on the ui panel
/// </summary>
public class CreditTextHandler : MonoBehaviour
{
    [SerializeField, Tooltip("text mesh pro")]
    private TextMeshPro _textMeshPro;


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
        _textMeshPro.text = textReader.ReadData(Config._creditsTextPath);
    }
}
