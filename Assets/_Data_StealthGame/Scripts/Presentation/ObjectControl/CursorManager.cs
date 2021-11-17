﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control direction and status of cursor
/// depending on the result of searching enemies and player's objects
/// </summary>
public class CursorManager : MonoBehaviour, ICursor
{
    [SerializeField, Tooltip("guide transform to spawn projectiles")]
    private Transform _spawnGuide;

    [SerializeField, Tooltip("transform of the main camera")]
    private Transform _cameraTransform;

    [SerializeField, Tooltip("max cursor distance")]
    private float _maxDistance = 3f;

    [SerializeField, Tooltip("cursor prefab")]
    private GageCursorHandler _cursorPrefab;

    [SerializeField, Tooltip("layer mask for object detection")]
    private LayerMask _layerMask;

    // instantiated cursor
    private GageCursorHandler _gageCursorHandler;

    // raycast hit 
    private RaycastHit _hit;

    // whether the ray hits something in the last frame
    private bool _lastFrameHitStatus;

    // whether the cursor object is enabled
    private bool _isEnabled;


    /// <summary>
    /// try to find objects, update cursor depending on the result of object searching
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }
        if (!_gageCursorHandler) { return; }
        
        // search any objects
        if(Physics.Raycast(_spawnGuide.position, _spawnGuide.forward, out _hit, _maxDistance, _layerMask.value, QueryTriggerInteraction.Collide))
        {
            // update position, animation and the hit status
            _gageCursorHandler.transform.position = _hit.point;
            // if the object is found in this frame, scale the cursor up
            if (!_lastFrameHitStatus)
            {
                _gageCursorHandler.ScaleUp();
            }
            _lastFrameHitStatus = true;
        }
        else
        {
            // update position, animation and the hit status
            _gageCursorHandler.transform.position = _spawnGuide.position + _spawnGuide.forward * _maxDistance;
            // if the object is lost in this frame, scale the cursor down
            if (_lastFrameHitStatus)
            {
                _gageCursorHandler.ScaleDown();
            }
            _lastFrameHitStatus = false;
        }

        // update rotation
        _gageCursorHandler.transform.LookAt(_cameraTransform, Vector3.up);
    }

    /// <summary>
    /// enable/disable cursor
    /// </summary>
    /// <param name="state"></param>
    public void SetActive(bool state)
    {
        _isEnabled = state;

        // when CursorManager is activated first time, spawn cursor object
        if(_isEnabled && !_gageCursorHandler)
        {
            InitCursor();
        }

        if (_gageCursorHandler)
        {
            _gageCursorHandler.gameObject.SetActive(_isEnabled);
        }
        
    }

    /// <summary>
    /// initialize cursor object
    /// </summary>
    private void InitCursor()
    {
        _gageCursorHandler = Instantiate(_cursorPrefab, _spawnGuide.position + _spawnGuide.forward * _maxDistance, _spawnGuide.rotation, transform);
    }
}
