/// <summary>
/// dataset of texts to be translated by the language setting
/// </summary>
[System.Serializable]
public class TextTranslationDataSet
{
    // identifier of the text to be translated
    public int _id;

    // English text
    public string _en;

    // Japanese text
    public string _jp;

    // description of the text
    public string _label;
}
