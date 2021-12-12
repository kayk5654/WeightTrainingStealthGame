using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control direction and status of cursor
/// depending on the result of searching enemies and player's objects
/// </summary>
public class CursorManager : MonoBehaviour, ICursor
{
    [SerializeField, Tooltip("scene object reference")]
    private SceneObjectContainer _sceneObjectContainer;

    [SerializeField, Tooltip("guide transform to spawn projectiles")]
    private Transform _spawnGuide;

    [SerializeField, Tooltip("transform of the main camera")]
    private Transform _cameraTransform;

    [SerializeField, Tooltip("max cursor distance")]
    private float _maxDistance = 4.5f;

    [SerializeField, Tooltip("cursor prefab")]
    private GageCursorHandler _cursorPrefab;

    // get look direction
    private ILookDirectionGetter _lookDirectionGetter;

    // get snapped cursor position
    private CursorSnapper _cursorSnapper;

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

    // whether the ring gage should be filled
    private bool _fillRingGage;

    // fill amount of the ring gage
    private float _ringGageFillAmount;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lookDirectionGetter = _sceneObjectContainer.GetLookDirectionGetter();
        _cursorSnapper = _sceneObjectContainer.GetCursorSnapper();
        InitGageFillingStatusSwitching();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        DisableGageFillingStatusSwitching();
    }

    /// <summary>
    /// try to find objects, update cursor depending on the result of object searching
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }
        if (!_gageCursorHandler) { return; }

        // update cursor transform
        UpdateCursorTransform();

        // fill ring gage
        FillRingGage();
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
        _gageCursorHandler = Instantiate(_cursorPrefab, _lookDirectionGetter.GetOrigin() + _lookDirectionGetter.GetDirection() * Config._cursorMaxDistance, Quaternion.identity, transform);
        _gageCursorHandler.transform.LookAt(_cameraTransform, Vector3.up);
        _cursorSnapper.Init(_gageCursorHandler.transform.position);
    }

    /// <summary>
    /// update cursor transform
    /// </summary>
    private void UpdateCursorTransform()
    {
        // search any objects
        bool isHit = Physics.Raycast(_lookDirectionGetter.GetOrigin(), _lookDirectionGetter.GetDirection(), out _hit, _maxDistance, _layerMask.value, QueryTriggerInteraction.Collide);

        // get snapped cursor position
        _cursorSnapper.SetRayCastingInfo(_lookDirectionGetter.GetOrigin(), _lookDirectionGetter.GetDirection(), isHit, _hit);

        // update position of the cursor
        _gageCursorHandler.transform.position = _cursorSnapper.GetSnappedCursorPosition();

        if (isHit)
        {
            // animation and the hit status
            // if the object is found in this frame, scale the cursor up
            if (!_lastFrameHitStatus)
            {
                _gageCursorHandler.ScaleUp();
            }
            _lastFrameHitStatus = true;

            // notify that the hittable target is found
            _sceneObjectContainer.GetProjectileTargetFinder().SetTargetObject(_hit.collider.transform);
        }
        else
        {
            // update animation and the hit status
            // if the object is lost in this frame, scale the cursor down
            if (_lastFrameHitStatus)
            {
                _gageCursorHandler.ScaleDown();
            }
            _lastFrameHitStatus = false;

            // notify that the hittable target is lost
            _sceneObjectContainer.GetProjectileTargetFinder().SetTargetObject(null);
        }

        // update rotation
        _gageCursorHandler.transform.LookAt(_cameraTransform, Vector3.up);
    }

    /// <summary>
    /// set start/stop gage filling as callbacks of the exercise input 
    /// </summary>
    private void InitGageFillingStatusSwitching()
    {
        _sceneObjectContainer.GetExerciseInput()._onStartHold += StartGageFilling;
        _sceneObjectContainer.GetExerciseInput()._onStopHold += StopGageFilling;
    }

    /// <summary>
    /// remove start/stop gage filling from the event of the exercise input
    /// </summary>
    private void DisableGageFillingStatusSwitching()
    {
        _sceneObjectContainer.GetExerciseInput()._onStartHold -= StartGageFilling;
        _sceneObjectContainer.GetExerciseInput()._onStopHold -= StopGageFilling;
    }

    /// <summary>
    /// start filling the ring gage
    /// </summary>
    private void StartGageFilling(object sender, EventArgs args)
    {
        _fillRingGage = true;
        _ringGageFillAmount = 0f;
        _gageCursorHandler.SetRingGageAngle(Mathf.Clamp01(_ringGageFillAmount));
        _sceneObjectContainer.GetProjectileTargetFinder().EnableFinding();
    }

    /// <summary>
    /// stop filling the ring gage
    /// </summary>
    private void StopGageFilling(object sender, EventArgs args)
    {
        _fillRingGage = false;
        _sceneObjectContainer.GetProjectileTargetFinder().DisableFinding();
    }

    /// <summary>
    /// fill the ring gage while the player is "holding"
    /// </summary>
    private void FillRingGage()
    {
        if (_fillRingGage) 
        {
            _ringGageFillAmount = Mathf.Clamp01(_ringGageFillAmount + Time.deltaTime / Config._ringGageFillDuration);
        }
        else
        {
            _ringGageFillAmount = Mathf.Clamp01(_ringGageFillAmount - Time.deltaTime / Config._ringGageFillDuration * 2f);
        }
        
        _gageCursorHandler.SetRingGageAngle(_ringGageFillAmount);
    }
}
