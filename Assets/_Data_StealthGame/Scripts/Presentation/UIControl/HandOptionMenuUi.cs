using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control option menu on a hand during gameplay
/// </summary>
public class HandOptionMenuUi : MonoBehaviour, IOptionMenuUi
{
    // event called when the gameplay is paused/resumed/terminated
    public event EventHandler<GamePlayStateEventArgs> _onGameplayStateChange;

    // whether the gameplay is pausing or not
    private bool _isPausing;

    [SerializeField, Tooltip("root of the buttons")]
    private GameObject _buttonsRoot;
    
    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        EnableUi(false);
    }

    /// <summary>
    /// enable option menu ui
    /// </summary>
    public void EnableUi(bool state)
    {
        _buttonsRoot.SetActive(state);
    }

    /// <summary>
    /// terminate gameplay and go back to the main menu; called by a pressable button
    /// </summary>
    public void BackToMenu()
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.None);
        _onGameplayStateChange?.Invoke(this, args);

        // revert pausing status to the default
        _isPausing = false;

        // hide buttons
        _buttonsRoot.SetActive(false);
    }

    public void TogglePauseResume()
    {
        if (_isPausing)
        {
            // resume gameplay
            GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.Playing);
            _onGameplayStateChange?.Invoke(this, args);
        }
        else
        {
            // pause gameplay
            GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.Pause);
            _onGameplayStateChange?.Invoke(this, args);
        }

        _isPausing = !_isPausing;
    }
}
