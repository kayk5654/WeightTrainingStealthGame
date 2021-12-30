using UnityEngine;
/// <summary>
/// this class sets transform of the UI canvas
/// </summary>
public class UITransformModifier : MonoBehaviour {

    // cameraTransform is assigned by GuidingContentManager class
    public Transform _cameraTransform;

    public enum TransformStyle { rotateAroundYAxis, staticAndLookAt, moveTowardsPlayer, rotateAroundXAndYAxis, fixPosOnEnable}
    [SerializeField]
    private TransformStyle _transformStyle;

    [Tooltip("distance from the camera")]
    public float _dist;
    [Tooltip("relative height from the camera")]
    public float _relativeHeight;
    [Tooltip("local x axis angle")]
    public float _localXAngle;
    [Tooltip("threshold of look direction and direction from the camera to the ui")]
    public float _lookDirDiffThresh = 0.8f;
    [Tooltip("threshold of height difference of the ui and the camera")]
    public float _heightDiffThresh = 0.1f;
    private Vector3 _angleInEulerAndDegrees;

    private bool _shouldCanvasTrackPlayer;
    private Vector3 _targetPos;

    // vector for calculation
    private Vector3 _tempVector;

    private void Start()
    {
        InitTargetPos();
    }

    private void InitTargetPos()
    {
        _targetPos = transform.position;
    }

    private void Update () {

        if(_cameraTransform == null) { return; }

        TrackCamera();
        RotateAroundPlayer();
        StaticPosLookAt();
        MoveTowardsPlayer();
        RotateAroundXAndYAxis();
    }
    
    private void StaticPosLookAt()
    {
        if (_transformStyle != TransformStyle.staticAndLookAt) { return; }

        LookAtPlayerXZ();
    }

    private void MoveTowardsPlayer()
    {
        if (_transformStyle != TransformStyle.moveTowardsPlayer) { return; }

        // rotation adjustment is always active

        LookAtPlayerXZ();

        // position adjustment is activated when the canvas is almost outside of the player's eyesight
        
        bool whenPosModActivated = GetLookDirDiffXZ() < _lookDirDiffThresh || GetHeightDiff() > _heightDiffThresh || GetDistanceDiff() > _dist;

        if (!whenPosModActivated) { return; }
        SetTrackTargetXZ();
        _shouldCanvasTrackPlayer = true;
    }

    private void RotateAroundPlayer()
    {
        if (_transformStyle != TransformStyle.rotateAroundYAxis) { return; }

        ConvertAngle();
        SetPositionXZ();
    }

    private void RotateAroundXAndYAxis()
    {
        if (_transformStyle != TransformStyle.rotateAroundXAndYAxis) { return; }

        LookAtPlayer3D();

        bool whenPosModActivated = GetLookDirDiff3D() < _lookDirDiffThresh || GetHeightDiff() > _heightDiffThresh;

        if (!whenPosModActivated) { return; }

        SetTrackTarget3D();
        _shouldCanvasTrackPlayer = true;
    }

    private void LookAtPlayerXZ()
    {
        // calculate forward vector
        _tempVector = _cameraTransform.position - transform.position;
        _tempVector.y = 0f;
        transform.rotation = Quaternion.LookRotation(-_tempVector) * Quaternion.Euler(_localXAngle, _angleInEulerAndDegrees.y, 0f);
    }

    private void LookAtPlayer3D()
    {
        // calculate direction from this transform to the camera
        _tempVector = _cameraTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-_tempVector);
    }

    private void SetTrackTargetXZ()
    {
        float relativeYPos = _relativeHeight + _cameraTransform.position.y;
        // calculate forward vector along XZ plane
        _tempVector = new Vector3(_cameraTransform.forward.x, 0f, _cameraTransform.forward.z);
        _targetPos = new Vector3(_cameraTransform.position.x, relativeYPos, _cameraTransform.position.z) + _tempVector * _dist;
    }

    private void SetTrackTarget3D()
    {
        _targetPos = _cameraTransform.position + _cameraTransform.forward * _dist;
    }

    private void TrackCamera()
    {
        if (!_shouldCanvasTrackPlayer) { return; }

        float lerpFactor = 0.2f;
        transform.position = Vector3.Lerp(transform.position, _targetPos, lerpFactor);

        float distDiffThresh = 0.01f;
        bool whenStopTrackingCamera = Vector3.Distance(transform.position, _targetPos) < distDiffThresh;

        if (!whenStopTrackingCamera) { return; }
        _shouldCanvasTrackPlayer = false;
    }

    private float GetLookDirDiffXZ()
    {
        Vector2 XZForwardVec = new Vector2(_cameraTransform.forward.x, _cameraTransform.forward.z).normalized;
        Vector2 PlayerToCanvasDir = new Vector2((transform.position - _cameraTransform.position).x, (transform.position - _cameraTransform.position).z).normalized;

        return Vector2.Dot(XZForwardVec, PlayerToCanvasDir);
    }

    private float GetDistanceDiff()
    {
        _tempVector = transform.position - _cameraTransform.position;
        _tempVector.y = 0f;
        return _tempVector.magnitude;
    }

    private float GetLookDirDiff3D()
    {
        // calculate direction from player to this ui
        _tempVector = (transform.position - _cameraTransform.position).normalized;

        return Vector3.Dot(_cameraTransform.forward, _tempVector);
    }
    
    private float GetHeightDiff()
    {
        return Mathf.Abs(transform.position.y - (_cameraTransform.position.y + _relativeHeight));
    }

    private void ConvertAngle()
    {
        _angleInEulerAndDegrees = _cameraTransform.rotation.eulerAngles;
    }

    private void FaceCanvas()
    {
        transform.localRotation = Quaternion.Euler(_localXAngle, _angleInEulerAndDegrees.y, 0f);
    }

    private void SetPositionXZ()
    {
        // calculate normalized forward vector along XZ plane of the camera
        _tempVector = new Vector3(_cameraTransform.forward.x, 0f, _cameraTransform.forward.z).normalized;
        float x = _tempVector.x * _dist + _cameraTransform.position.x;
        float y = _relativeHeight + _cameraTransform.position.y;
        float z = _tempVector.z * _dist + _cameraTransform.position.z;

        transform.position = new Vector3(x, y, z);
    }

    private void SetPosition3D()
    {
        // calculate normalized forward vector along XZ plane of the camera
        _tempVector = new Vector3(_cameraTransform.forward.x, 0f, _cameraTransform.forward.z).normalized;
        transform.position = _tempVector * _dist + _cameraTransform.position;
    }

    private void OnEnable()
    {
        FixPosOnEnable();
    }

    private void FixPosOnEnable()
    {
        if(_transformStyle != TransformStyle.fixPosOnEnable) { return; }
        
        SetPositionXZ();
        ConvertAngle();
        FaceCanvas();
    }

    public void Reset()
    {
        if (_transformStyle == TransformStyle.fixPosOnEnable)
        {
            FixPosOnEnable();
        }
    }
}
