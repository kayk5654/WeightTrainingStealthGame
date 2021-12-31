using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// switch text depending on the language settings
/// </summary>
public class TextTranslationManager : MonoBehaviour
{
    // texts to translate
    private TranslatedText[] _translatedTexts;

    // database of texts
    private TextTranslationDatabase _textTranslationDatabase;


    /// <summary>
    /// initialization
    /// </summary>
    private void Awake()
    {
        _textTranslationDatabase = new TextTranslationDatabase(new JsonDatabaseReader<TextTranslationDataSet>(), Config._textTranslationDataPath);
        _translatedTexts = Resources.FindObjectsOfTypeAll(typeof(TranslatedText)) as TranslatedText[];
        _translatedTexts = _translatedTexts.Where(text => !string.IsNullOrEmpty(text.gameObject.scene.name)).ToArray();
        SetLanguage(Config._english);
    }

    /// <summary>
    /// debugging
    /// </summary>
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetLanguage(Config._english);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetLanguage(Config._japanese);
        }
        */
    }

    /// <summary>
    /// set language of the texts
    /// </summary>
    public void SetLanguage(string language)
    {
        TextTranslationDataSet tempTranslationDataset;
        
        foreach (TranslatedText text in _translatedTexts)
        {
            tempTranslationDataset = _textTranslationDatabase.GetData(text.GetId());
            
            if (language == Config._english)
            {
                text.SetText(tempTranslationDataset._en);
            }
            else if (language == Config._japanese)
            {
                text.SetText(tempTranslationDataset._jp);
            }
        }
    }
}
