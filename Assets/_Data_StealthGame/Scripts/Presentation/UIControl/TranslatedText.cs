using TMPro;
using UnityEngine;
/// <summary>
/// text to translate
/// </summary>
public class TranslatedText : MonoBehaviour
{
    [SerializeField, Tooltip("text box")]
    protected TextMeshPro _textMeshPro;

    [SerializeField, Tooltip("id of text")]
    protected int _id;

    [SerializeField, Tooltip(" type of text")]
    protected TextType _textType;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// set text to display
    /// </summary>
    /// <param name="language"></param>
    public virtual void SetText(string text)
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
    public virtual int GetId()
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
