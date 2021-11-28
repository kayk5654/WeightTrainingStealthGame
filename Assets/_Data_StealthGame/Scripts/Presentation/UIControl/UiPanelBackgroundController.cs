using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// display/hide background of the main ui panel
/// </summary>
public class UiPanelBackgroundController : MonoBehaviour
{
    [SerializeField, Tooltip("background object")]
    private GameObject _background;

    [SerializeField, Tooltip("other ui panels")]
    private GameObject[] _panels;


    /// <summary>
    /// display/hide background
    /// </summary>
    private void Update()
    {
        SetBackgroundVisibleState();
    }

    /// <summary>
    /// display/hide background depending on the visibility of other ui panels
    /// </summary>
    private void SetBackgroundVisibleState()
    {
        _background.SetActive(AreAnyPanelsVisible());
    }

    /// <summary>
    /// whether there are any visible panels currently
    /// </summary>
    /// <returns></returns>
    private bool AreAnyPanelsVisible()
    {
        bool isVisible = false;
        foreach (GameObject panel in _panels)
        {
            if (panel.activeSelf)
            {
                isVisible = true;
            }
        }

        return isVisible;
    }
}
