using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control single warning icon
/// </summary>
public class WarningIcon : MonoBehaviour
{
    // transform of the relative node
    private Transform _relativeNode;

    // transform of the main camera
    private Transform _cameraTransform;

    // id of the relative node
    private int _relativeNodeId = -1;

    [SerializeField, Tooltip("line to connect this icon and relative node")]
    private LineRenderer _lineRenderer;

    // threshold to compare angle of view direction of this icon and view direction of the relative node
    private float _dotThreshold = 0.99f;

    // distance from the center of the icon placement area of the parent of this icon
    private float _distFromCenter;

    // for calculation of the local position
    private Vector3 _localPositionTemp = Vector3.zero;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// update status of this icon
    /// </summary>
    private void Update()
    {
        // if this icon isn't initialized, do nothing
        if(_relativeNodeId == -1) { return; }
        
        // update position
        CalculateIconPosition();

        // draw line connecting this icon and the relative node properly
        UpdateLine();

        // if the relative node is looked, hide this icon
        if (IsRelativeNodeLooked())
        {
            StartCoroutine(HideIconSequence());
        }

        // if the relative node is destroyed delete this icon
        if (IsRelativeNodeDestroyed())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// set transform of the relative node
    /// </summary>
    /// <param name="nodeTransorm"></param>
    public void SetRelativeNodeTransform(Transform nodeTransorm)
    {
        _relativeNode = nodeTransorm;
    }

    /// <summary>
    /// set transform of the main camera
    /// </summary>
    /// <param name="cameraTransform"></param>
    public void SetCameraTransform(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    /// <summary>
    /// set id of the relative node
    /// </summary>
    /// <param name="nodeId"></param>
    public void SetRelativeNodeId(int nodeId)
    {
        _relativeNodeId = nodeId;
    }

    /// <summary>
    /// get id of the relative node
    /// </summary>
    /// <returns></returns>
    public int GetRelativeNodeId()
    {
        return _relativeNodeId;
    }

    /// <summary>
    /// set distance from center of the icon placement area
    /// </summary>
    /// <param name="dist"></param>
    public void SetDistFromCenter(float dist)
    {
        _distFromCenter = dist;
    }

    /// <summary>
    /// check whether the relative node is looked
    /// </summary>
    /// <returns></returns>
    private bool IsRelativeNodeLooked()
    {
        return Vector3.Dot((transform.position - _cameraTransform.position).normalized, (_relativeNode.position - _cameraTransform.position)) > _dotThreshold;
    }

    /// <summary>
    /// draw line connecting this icon and the relative node properly
    /// </summary>
    private void UpdateLine()
    {
        if(!_relativeNode) { return; }

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _relativeNode.position);
    }

    /// <summary>
    /// calculate local position of this icon
    /// </summary>
    private void CalculateIconPosition()
    {
        if (!_cameraTransform || !_relativeNode) { return; }

        _localPositionTemp = transform.parent.InverseTransformDirection((_relativeNode.position - _cameraTransform.position).normalized);
        _localPositionTemp.z = 0f;
        _localPositionTemp = _localPositionTemp.normalized;
        transform.localPosition = _localPositionTemp * _distFromCenter;
    }

    /// <summary>
    /// hide this icon after certain period of time
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideIconSequence()
    {
        float hidePeriodDuration = 3f;
        float phase = 0f;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while(phase < 1f)
        {
            phase += Time.deltaTime / hidePeriodDuration;

            yield return waitForEndOfFrame;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// check whether the relative node is destroyed
    /// </summary>
    /// <returns></returns>
    private bool IsRelativeNodeDestroyed()
    {
        return _relativeNode == null;
    }
}
