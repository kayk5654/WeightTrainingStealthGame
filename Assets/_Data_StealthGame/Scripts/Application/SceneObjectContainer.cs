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

    [Header("Look Direction Getter")]
    [SerializeField, Tooltip("type of look direction getter")]
    private LookDirectionGetterType _lookDirectionGetterType;

    [SerializeField, Tooltip("look direction getter using camera transform")]
    private CameraLookDirectionGetter _cameraLookDirectionGetter;

    [SerializeField, Tooltip("look direction getter using eye gaze feature in MRTK")]
    private MRTKLookDirectionGetter _mrtkLookDirectionGetter;

    [SerializeField, Tooltip("get snapped cursor position")]
    private CursorSnapper _cursorSnapper;

    [SerializeField, Tooltip("search and find target object to shoot projectiles toward")]
    private ProjectileTargetFinder _projectileTargetFinder;

    [SerializeField, Tooltip("get exercise input")]
    private ExerciseInput _exerciseInput;


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
                return _mrtkLookDirectionGetter;

            default:
                return null;
        }
        
    }

    /// <summary>
    /// get cursor snapper
    /// </summary>
    /// <returns></returns>
    public CursorSnapper GetCursorSnapper()
    {
        return _cursorSnapper;
    }

    /// <summary>
    /// get projectile target finder
    /// </summary>
    /// <returns></returns>
    public ProjectileTargetFinder GetProjectileTargetFinder()
    {
        return _projectileTargetFinder;
    }

    /// <summary>
    /// get exercise input
    /// </summary>
    /// <returns></returns>
    public ExerciseInput GetExerciseInput()
    {
        return _exerciseInput;
    }
}
