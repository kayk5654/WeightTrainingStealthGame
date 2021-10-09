using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control gage-cursor object
/// </summary>
public class GageCursorHandler : MonoBehaviour
{
    [SerializeField, Tooltip("root transform")]
    private Transform _root;

    [SerializeField, Tooltip("gage transform")]
    private MeshRenderer _gage;

    [SerializeField, Tooltip("cursor plane mesh renderer")]
    private MeshRenderer _cursorPlane;

    // material for the cursor plane
    private Material _cursorMaterial;

    // material for the ring gage
    private Material _gageMaterial;

    // property name of the radius of the cursor plane
    private string _cursorRadiusProperty = "_DisplayRadius";

    // property name of the displayed angle of the ring gage
    private string _gageAngleProperty = "_Angle";

    // vector3 for setting scale of the ring gage temporarily
    private Vector3 _tempGageScale;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _cursorMaterial = _cursorPlane.material;
        _gageMaterial = _gage.material;
    }

    /// <summary>
    /// set scale of the ring gage and cursor plane
    /// </summary>
    private void SetScale(float scale)
    {
        _cursorMaterial.SetFloat(_cursorRadiusProperty, scale);
        _tempGageScale.x = scale;
        _tempGageScale.y = scale;
        _tempGageScale.z = scale;
        _gage.transform.localScale = _tempGageScale;
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
