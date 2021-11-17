using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lookDirectionGetter = _sceneObjectContainer.GetLookDirectionGetter();
        _cursorSnapper = _sceneObjectContainer.GetCursorSnapper();
    }

    /// <summary>
    /// try to find objects, update cursor depending on the result of object searching
    /// </summary>
    private void Update()
    {
        if (!_isEnabled) { return; }
        if (!_gageCursorHandler) { return; }

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
        _gageCursorHandler = Instantiate(_cursorPrefab, _lookDirectionGetter.GetOrigin() + _lookDirectionGetter.GetDirection() * Config._cursorMaxDistance, Quaternion.identity, transform);
        _gageCursorHandler.transform.LookAt(_cameraTransform, Vector3.up);
        _cursorSnapper.Init(_gageCursorHandler.transform.position);
    }
}
