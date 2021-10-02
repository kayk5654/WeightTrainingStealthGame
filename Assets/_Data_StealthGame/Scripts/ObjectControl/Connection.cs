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

    // id of a node to connect
    private int _connectNode1 = -1;

    // id of a node to connect
    private int _connectNode2 = -1;

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
    /// set relative node ids to connect together when initialize this instance
    /// </summary>
    /// <param name="nodeId1"></param>
    /// <param name="nodeId2"></param>
    public void SetNodeIds(int nodeId1, int nodeId2)
    {
        _connectNode1 = nodeId1;
        _connectNode2 = nodeId2;
    }

    /// <summary>
    /// set position of nodes to connect
    /// </summary>
    public void SetNodesPosition(Vector3 nodePos1, Vector3 nodePos2)
    {
        _lineRenderer.SetPosition(0, nodePos1);
        _lineRenderer.SetPosition(1, nodePos2);
    }

    /// <summary>
    /// returns _id of another side of node
    /// if selfNodeId is irrelevant, return -1
    /// </summary>
    /// <param name="selfNodeId"></param>
    /// <returns></returns>
    public int GetAnotherNode(int selfNodeId)
    {
        if(selfNodeId == _connectNode1)
        {
            return _connectNode2;
        }
        else if(selfNodeId == _connectNode2)
        {
            return _connectNode1;
        }
        else
        {
            return -1;
        }
    }

    // TODO: control material to fade line, change color of line, etc.
}
