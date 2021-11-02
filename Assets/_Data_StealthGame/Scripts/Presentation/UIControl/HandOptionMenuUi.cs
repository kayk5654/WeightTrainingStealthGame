using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control option menu on a hand during gameplay
/// </summary>
public class HandOptionMenuUi : MonoBehaviour, IOptionMenuUi
{
    // event called when the gameplay is paused
    public event EventHandler _onPause;

    // event called when the gameplay is resumed
    public event EventHandler _onResume;

    // event called when the gameplay is terminated and go back to the main menu
    public event EventHandler _onBackToMenu;

    // whether the gameplay is pausing or not
    private bool _isPausing;

    [SerializeField, Tooltip("root of the buttons")]
    private GameObject _buttonsRoot;
    
    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _buttonsRoot.SetActive(false);
    }

    /// <summary>
    /// enable option menu ui
    /// </summary>
    public void EnableUi()
    {
        _buttonsRoot.SetActive(true);
    }

    /// <summary>
    /// terminate gameplay and go back to the main menu; called by a pressable button
    /// </summary>
    public void BackToMenu()
    {
        EventArgs args = EventArgs.Empty;
        _onBackToMenu?.Invoke(this, args);

        // revert pausing status to the default
        _isPausing = false;

        // hide buttons
        _buttonsRoot.SetActive(false);
    }

    public void TogglePauseResume()
    {
        EventArgs args = EventArgs.Empty;
        
        if (_isPausing)
        {
            // resume gameplay
            _onResume?.Invoke(this, args);
        }
        else
        {
            // pause gameplay
            _onPause?.Invoke(this, args);
        }

        _isPausing = !_isPausing;
    }
}
