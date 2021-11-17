using System.Collections;
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

    [Header("Look Direction Getter")]
    [SerializeField, Tooltip("type of look direction getter")]
    private LookDirectionGetterType _lookDirectionGetterType;

    [SerializeField, Tooltip("look direction getter using camera transform")]
    private CameraLookDirectionGetter _cameraLookDirectionGetter;

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

    /// <summary>
    /// get look direction getter
    /// </summary>
    /// <returns></returns>
    public ILookDirectionGetter GetLookDirectionGetter()
    {
        switch (_lookDirectionGetterType)
        {
            case LookDirectionGetterType.CameraTransform:
                return _cameraLookDirectionGetter;

            case LookDirectionGetterType.MRTK:
                return null;

            default:
                return null;
        }
        
    }
}
