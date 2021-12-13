﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// phase of tutorial
/// </summary>
public class TutorialUiPhase : MonoBehaviour, IUiPhase
{
    // notify the action to move to the selected phase
    public event EventHandler<UiPhaseEventArgs> _onMoveToSelectedPhase;

    [SerializeField, Tooltip("identify role of this phase")]
    private TutorialPhase _phaseType;

    [SerializeField, Tooltip("root gameobject of this ui phase")]
    private GameObject _panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public void Display()
    {
        _panel.SetActive(true);
    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public void Hide()
    {
        _panel.SetActive(false);
    }

    /// <summary>
    /// execute process to go to the selected phase from a button 
    /// </summary>
    /// <param name="phaseId"></param>
    public void MoveToSelectedPhase(int phaseId)
    {

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
