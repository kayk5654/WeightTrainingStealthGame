using TMPro;
using UnityEngine;
/// <summary>
/// text to translate
/// </summary>
public class TranslatedText : MonoBehaviour
{
    [SerializeField, Tooltip("text box")]
    private TextMeshPro _textMeshPro;

    [SerializeField, Tooltip("id of text")]
    private int _id;

    [SerializeField, Tooltip(" type of text")]
    private TextType _textType;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// set text to display
    /// </summary>
    /// <param name="language"></param>
    public void SetText(string text)
    {
        _textMeshPro.text = text;
    }

    /// <summary>
    /// set font to display
    /// </summary>
    /// <param name="font"></param>
    public void SetFont(TMP_FontAsset font)
    {
        _textMeshPro.font = font;
    }

    /// <summary>
    /// check id of this text
    /// </summary>
    /// <returns></returns>
    public int GetId()
    {
        return _id;
    }

    /// <summary>
    /// check text type of this text
    /// </summary>
    /// <returns></returns>
    public TextType GetTextType()
    {
        return _textType;
    }
}
