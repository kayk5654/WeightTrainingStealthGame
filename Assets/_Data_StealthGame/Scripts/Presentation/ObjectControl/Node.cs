using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// contain parameters of node
/// </summary>
public class Node : MonoBehaviour
{
    // reference of NodesManager
    private NodesManager _nodesManager;

    [Tooltip("id of each nodes")]
    private int _id = -1;

    // speed to extend lines to connect
    private float _speed = 1f;

    [SerializeField, Tooltip("node mesh")]
    private MeshRenderer _mainMeshRenderer;

    // material of node mesh
    private Material _nodeMaterial;

    [SerializeField, Tooltip("node cap prefab to instantiate")]
    private Transform _nodeCapPrefab;

    // node caps duplicated for each connections
    private List<Transform> _nodeCaps = new List<Transform>();

    // index of relative connections
    private List<int> _connectionsIds = new List<int>();

    // for corouines
    WaitForEndOfFrame _waitForEndOfFrame;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _nodeMaterial = _mainMeshRenderer.material;
        WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    private void Update()
    {
        UpdateNodeCapTransform();
    }

    /// <summary>
    /// initialize parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nodesManager"></param>
    /// <param name="speed"></param>
    public void InitParams(int id, NodesManager nodesManager, float speed)
    {
        _id = id;
        _nodesManager = nodesManager;
        _speed = speed;
    }

    /// <summary>
    /// spawn node cap of this node from another node
    /// </summary>
    private void SpawnNodeCap(Transform lookAtTarget)
    {
        Transform newNodeCap = Instantiate(_nodeCapPrefab, transform).transform;
        newNodeCap.LookAt(lookAtTarget);
        newNodeCap.gameObject.SetActive(true);
        _nodeCaps.Add(newNodeCap);
    }

    /// <summary>
    /// initialize node caps; spawn and set direction for each connections
    /// </summary>
    public void InitializeNodeCaps()
    {
        for(int i = 0; i < _connectionsIds.Count; i++)
        {
            int anotherNodeId = _nodesManager.GetConnection(_connectionsIds[i]).GetAnotherNode(_id);
            SpawnNodeCap(_nodesManager.GetNode(anotherNodeId).transform);
        }
    }

    /// <summary>
    /// set simulated result on the NodeConnectionControl.compute
    /// </summary>
    public void SetSimulatedData(Node_ComputeShader nodeData)
    {
        if(nodeData._id != _id) { return; }
        
        transform.position = nodeData._position;
    }

    /// <summary>
    /// update direction of node caps
    /// </summary>
    private void UpdateNodeCapTransform()
    {
        // index of _nodeCaps matches index of _connectionsIds
        // see InitializeNodeCaps()
        for (int i = 0; i < _nodeCaps.Count; i++)
        {
            int anotherNodeId = _nodesManager.GetConnection(_connectionsIds[i]).GetAnotherNode(_id);
            _nodeCaps[i].LookAt(_nodesManager.GetNode(anotherNodeId).transform);
        }
    }

    /// <summary>
    /// add relative connection to _connectionsIndex
    /// </summary>
    /// <param name="connectionId"></param>
    public void AddConnection(int connectionId)
    {
        if (_connectionsIds.Contains(connectionId)) { return; }
        _connectionsIds.Add(connectionId);
    }

    /// <summary>
    /// remove relative connection from _connectionsIndex
    /// </summary>
    /// <param name="connectionId"></param>
    public void RemoveConnection(int connectionId)
    {
        if (!_connectionsIds.Contains(connectionId)) { return; }
        _connectionsIds.Remove(connectionId);
    }
}
