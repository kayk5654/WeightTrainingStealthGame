﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control ring gage to start workout
/// </summary>
public class RingGageHandler : MonoBehaviour
{
    [SerializeField, Tooltip("shared references")]
    private SceneObjectContainer _sceneObjectContainer;

    [SerializeField, Tooltip("gage transform")]
    private MeshRenderer _gage;

    // look time checker to start gameplay
    private LookTimeChecker _lookTimeChecker;

    // material for the ring gage
    private Material _gageMaterial;

    // property name of the displayed angle of the ring gage
    private string _gageAngleProperty = "_Angle";


    /// <summary>
    /// initialize
    /// </summary>
    private void Start()
    {
        _lookTimeChecker = _sceneObjectContainer.GetLookTimeChecker();
        _gageMaterial = _gage.material;
    }

    /// <summary>
    /// update filling status of the ring gage
    /// </summary>
    private void Update()
    {
        SetRingGageAngle(_lookTimeChecker.GetNormalizedLookTime());
    }

    /// <summary>
    /// set filled angle of the ring gage
    /// </summary>
    /// <param name="angle"></param>
    private void SetRingGageAngle(float angle)
    {
        _gageMaterial.SetFloat(_gageAngleProperty, angle);
    }
}