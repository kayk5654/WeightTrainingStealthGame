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

    [SerializeField, Tooltip("animator of the background")]
    private Animator _animator;

    // animator property to open/close background panel
    private string _openPanelAnimProperty = "Opened";

    // process to open/close background panel
    private IEnumerator _openCloseSequence;

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
        if (AreAnyPanelsVisible())
        {
            if(_openCloseSequence != null) { return; }
            _openCloseSequence = OpenPanelSequence();
            StartCoroutine(_openCloseSequence);
        }
        else
        {
            if (_openCloseSequence != null) { return; }
            _openCloseSequence = ClosePanelSequence();
            StartCoroutine(ClosePanelSequence());
        }

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

    /// <summary>
    /// process to open background panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenPanelSequence()
    {
        _background.SetActive(true);
        _animator.SetBool(_openPanelAnimProperty, true);
        yield return new WaitForSeconds(0.5f);
        _openCloseSequence = null;
    }

    /// <summary>
    /// process to close background panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClosePanelSequence()
    {
        _animator.SetBool(_openPanelAnimProperty, false);
        yield return new WaitForSeconds(0.5f);
        _background.SetActive(false);
        _openCloseSequence = null;
    }
}
