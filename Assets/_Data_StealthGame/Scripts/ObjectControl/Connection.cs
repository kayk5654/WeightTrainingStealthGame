using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control connection between nodes
/// </summary>
public class Connection : MonoBehaviour
{
    [Tooltip("id of each connections")]
    public int _id;

    [SerializeField, Tooltip("line renderer to draw connection")]
    private LineRenderer _lineRenderer;

    // material of _lineRenderer
    private Material _lineMaterial;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _lineMaterial = _lineRenderer.material;
    }

    /// <summary>
    /// set position of nodes to connect
    /// </summary>
    public void SetNodesPosition(Vector3 nodePos1, Vector3 nodePos2)
    {
        _lineRenderer.SetPosition(0, nodePos1);
        _lineRenderer.SetPosition(1, nodePos2);
    }

    // TODO: control material to fade line, change color of line, etc.
}
