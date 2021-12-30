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
    /// check id of this text
    /// </summary>
    /// <returns></returns>
    public int GetId()
    {
        return _id;
    }
}
