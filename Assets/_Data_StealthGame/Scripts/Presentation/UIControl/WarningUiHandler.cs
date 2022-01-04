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

    // manage spawned warning icons
    private List<WarningIcon> _warningIcons = new List<WarningIcon>();

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
        UpdateIconList();
    }

    /// <summary>
    /// display warning icon just after any nodes are attacked
    /// </summary>
    private void DisplayWarningIcon(object sender, InGameObjectEventArgs args)
    {
        WarningIcon newIcon = Instantiate(_warningIconPrefab, transform).GetComponent<WarningIcon>();
        newIcon.SetRelativeNodeTransform(_nodesManager.GetNode(args._id).transform);
        newIcon.SetCameraTransform(_cameraTransform);
        newIcon.SetRelativeNodeId(args._id);
        newIcon.SetDistFromCenter(_radius);
        _warningIcons.Add(newIcon);
    }

    /// <summary>
    /// update status of the icons
    /// </summary>
    private void UpdateIconList()
    {
        if (_warningIcons == null || _warningIcons.Count < 1) { return; }

        try
        {
            foreach (WarningIcon icon in _warningIcons)
            {
                // remove icon reference if the relative node is deleted
                if (icon == null)
                {
                    _warningIcons.Remove(icon);
                    continue;
                }
            }
        }
        catch
        {

        }
    }

    /// <summary>
    /// delete all spawned icons
    /// </summary>
    private void DeleteAllIcons()
    {
        foreach(WarningIcon icon in _warningIcons)
        {
            Destroy(icon.gameObject);
        }
        
        _warningIcons.Clear();
    }
}
