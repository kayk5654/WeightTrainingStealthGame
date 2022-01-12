using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
/// <summary>
/// dataset of fonts and language setting
/// </summary>
[System.Serializable]
public class FontSet
{
    // identifier of the relative language setting
    public string _language;
    
    // font for headers
    public TMP_FontAsset _headerFontAsset;
    
    // font for plain texts
    public TMP_FontAsset _plainTextFontAsset;
}

/// <summary>
/// switch text depending on the language settings
/// </summary>
public class TextTranslationManager : MonoBehaviour
{
    [SerializeField, Tooltip("list of fonts")]
    private FontSet[] _fontSets;

    // texts to translate
    private TranslatedText[] _translatedTexts;

    // database of texts
    private TextTranslationDatabase _textTranslationDatabase;

    // selected language
    private string _selectedLanguage = Config._english;


    /// <summary>
    /// initialization
    /// </summary>
    private void Awake()
    {
        _textTranslationDatabase = new TextTranslationDatabase(new JsonDatabaseReader<TextTranslationDataSet>(), Config._textTranslationDataPath);
        _translatedTexts = Resources.FindObjectsOfTypeAll(typeof(TranslatedText)) as TranslatedText[];
        _translatedTexts = _translatedTexts.Where(text => !string.IsNullOrEmpty(text.gameObject.scene.name)).ToArray();
        SetLanguage(_selectedLanguage);
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
        // record language
        _selectedLanguage = language;

        // set fonts
        FontSet tempFontSet = _fontSets.SingleOrDefault(set => set._language == _selectedLanguage);

        // set texts
        TextTranslationDataSet tempTranslationDataset;
        
        foreach (TranslatedText text in _translatedTexts)
        {
            // assign fonts
            if(text.GetTextType() == TextType.Header)
            {
                // assign font for headers
                text.SetFont(tempFontSet._headerFontAsset);
            }
            else
            {
                // assing font for plain texts
                text.SetFont(tempFontSet._plainTextFontAsset);
            }
            
            // assign texts
            tempTranslationDataset = _textTranslationDatabase.GetData(text.GetId());
            
            if (_selectedLanguage == Config._english)
            {
                text.SetText(tempTranslationDataset._en);
            }
            else if (_selectedLanguage == Config._japanese)
            {
                text.SetText(tempTranslationDataset._jp);
            }
        }
    }

    /// <summary>
    /// access specified text translation dataset
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTextTranslationData(int id)
    {
        TextTranslationDataSet tempTranslationDataset = _textTranslationDatabase.GetData(id);

        if (_selectedLanguage == Config._english)
        {
            return tempTranslationDataset._en;
        }
        else if (_selectedLanguage == Config._japanese)
        {
            return tempTranslationDataset._jp;
        }
        else
        {
            return null;
        }
    }
}
