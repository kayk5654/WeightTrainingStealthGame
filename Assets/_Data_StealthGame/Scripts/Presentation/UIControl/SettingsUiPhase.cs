﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// phase of settings
/// </summary>
public class SettingsUiPhase : MonoBehaviour, IUiPhase
{

    // notify the action to move to the selected phase
    public event EventHandler<UiPhaseEventArgs> _onMoveToSelectedPhase;

    [SerializeField, Tooltip("identify role of this phase")]
    protected SettingsPhase _phaseType;

    [SerializeField, Tooltip("root gameobject of this ui phase")]
    protected GameObject[] _rootObjects;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public void Display()
    {
        foreach (GameObject root in _rootObjects)
        {
            root.SetActive(true);
        }
    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public void Hide()
    {
        foreach (GameObject root in _rootObjects)
        {
            root.SetActive(false);
        }
    }

    /// <summary>
    /// execute process to go to the selected phase from a button 
    /// </summary>
    /// <param name="phaseId"></param>
    public void MoveToSelectedPhase(int phaseId)
    {
        UiPhaseEventArgs args = new UiPhaseEventArgs(phaseId);
        _onMoveToSelectedPhase.Invoke(this, args);
    }

    /// <summary>
    /// get phase id among the same ui phase group managed by IMultiPhaseUi
    /// </summary>
    /// <returns></returns>
    public int GetPhaseId()
    {
        return (int)_phaseType;
    }
}