using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control warning icons connected to one of the active nodes
/// </summary>
public class WarningUiHandler : MonoBehaviour
{
    [SerializeField, Tooltip("prefab of warning icon")]
    private GameObject _warningIconPrefab;

    [SerializeField, Tooltip("radious of icon placement area")]
    private float _radius = 0.3f;

    [SerializeField, Tooltip("nodes manager")]
    private NodesManager _nodesManager;

    [SerializeField, Tooltip("camera transform")]
    private Transform _cameraTransform;

    /// <summary>
    /// set callback
    /// </summary>
    private void Start()
    {
        _nodesManager._onNodeAttacked += DisplayWarningIcon;
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _nodesManager._onNodeAttacked -= DisplayWarningIcon;
    }

    /// <summary>
    /// clear warning icon list
    /// </summary>
    private void OnDisable()
    {
        DeleteAllIcons();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// display warning icon just after any nodes are attacked
    /// </summary>
    private void DisplayWarningIcon(object sender, InGameObjectEventArgs args)
    {
        WarningIcon newIcon = Instantiate(_warningIconPrefab, transform).GetComponent<WarningIcon>();
        newIcon.SetRelativeNodeTransform(_nodesManager.GetNode(args._id).transform);
        newIcon.SetCameraTransform(_cameraTransform);
        newIcon.SetDistFromCenter(_radius);
        newIcon.SetRelativeNodeId(args._id);
    }

    /// <summary>
    /// delete all spawned icons
    /// </summary>
    private void DeleteAllIcons()
    {
        WarningIcon[] icons = GetComponentsInChildren<WarningIcon>();
        
        foreach(WarningIcon icon in icons)
        {
            Destroy(icon.gameObject);
        }
    }
}
