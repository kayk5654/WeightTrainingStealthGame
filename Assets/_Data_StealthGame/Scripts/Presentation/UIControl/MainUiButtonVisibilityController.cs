using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// display/hide buttons on the main ui panel
/// </summary>
public class MainUiButtonVisibilityController : MonoBehaviour
{
    [SerializeField, Tooltip("buttons")]
    private GameObject[] _buttons;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        Hide();
    }

    /// <summary>
    /// display buttons
    /// </summary>
    public void Display()
    {
        foreach(GameObject button in _buttons)
        {
            button.SetActive(true);
        }
    }

    /// <summary>
    /// hide buttons
    /// </summary>
    public void Hide()
    {
        foreach (GameObject button in _buttons)
        {
            button.SetActive(false);
        }
    }
}
