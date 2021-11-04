﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// contain instances of classes for interaction in the scene,
/// so that the required references can be always taken from this class
/// </summary>
public class SceneObjectContainer : MonoBehaviour
{
    [SerializeField, Tooltip("in game offense action")]
    private IInGameOffenseAction _offenseAction;

    [SerializeField, Tooltip("in game defense action")]
    private IInGameDefenseAction _defenseAction;

    [SerializeField, Tooltip("main ui phases")]
    private MainUiPhase[] _mainUiPhases;

    /// <summary>
    /// get _offenseAction
    /// </summary>
    /// <returns></returns>
    public IInGameOffenseAction GetIInGameOffenseAction()
    {
        return _offenseAction;
    }

    /// <summary>
    /// get _defenseAction
    /// </summary>
    /// <returns></returns>
    public IInGameDefenseAction GetIInGameDefenseAction()
    {
        return _defenseAction;
    }

    /// <summary>
    /// get _mainUiPhases
    /// </summary>
    /// <returns></returns>
    public MainUiPhase[] GetMainUiPhaseArray()
    {
        return _mainUiPhases;
    }
}