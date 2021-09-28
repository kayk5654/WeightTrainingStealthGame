using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// data of single node
/// </summary>
public struct Node_ComputeShader
{
    // identify each nodes
    public int _id;
    // position of a node
    public Vector3 _position;
    // rotation of a node
    public Vector4 _rotation;
}

/// <summary>
/// data of connection between nodes
/// </summary>
public struct Connection_ComputeShader
{
    // identify each connections
    int _id;
    // node to connect
    int _connectNode1;
    // node to connect
    int _connectNode2;
}

/// <summary>
/// manages connected nodes
/// </summary>
public class NodesManager : MonoBehaviour
{
    [SerializeField, Tooltip("compute shader for node control")]
    private ComputeShader __nodeConnectionControl;

    [SerializeField, Tooltip("nodes without light")]
    private GameObject _nodePrefab;

    [SerializeField, Tooltip("area to spawn nodes")]
    private BoxCollider _spawnArea;

    [SerializeField, Tooltip("number of nodes")]
    private int _nodeCount = 100;

    // array of _nodeWithoutLight instances
    private Node[] _nodes;

    [SerializeField, Tooltip("material of line")]
    private Material _lineMaterial;

    [SerializeField, Tooltip("connecting speed")]
    private float _speed = 1f;

    [SerializeField, Tooltip("connecting range")]
    private float _range = 1f;

    // buffer for nodes
    private ComputeBuffer _nodesBuffer;

    // buffer for connections
    private ComputeBuffer _connectionBuffer;

    // data to set _nodesBuffer
    private Node_ComputeShader[] _nodesBufferData;

    // data to set _connectionBuffer
    private Connection_ComputeShader[] _connectionBufferData;

    // parameter name of _nodeCount
    private string _nodeCountParamName = "_nodeCount";

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        // test on cpu
        //SpawnNodes();
        //StartConnect();

        // test on gpu
        InitializeBuffers();
        InitializeParams();
        SpawnNodes_GPU();
    }

    /// <summary>
    /// delete nodes without light
    /// </summary>
    private void OnDisable()
    {
        for(int i = 0; i < _nodes.Length; i++)
        {
            Destroy(_nodes[i].gameObject);
        }

        // test on gpu
        ReleaseBuffers();
    }

    private void Update()
    {
        
    }

    #region Test on CPU

    /// <summary>
    /// generate nodes
    /// </summary>
    private void SpawnNodes()
    {
        _nodes = new Node[_nodeCount];
        Vector3 positionTemp = Vector3.zero;
        for (int i = 0; i < _nodeCount; i++)
        {
            Transform newNode = Instantiate(_nodePrefab).transform;
            positionTemp.x = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
            positionTemp.y = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
            positionTemp.z = Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);
            newNode.position = positionTemp;
            newNode.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            newNode.SetParent(this.transform);
            _nodes[i] = newNode.GetComponent<Node>();
            _nodes[i]._nodesManager = this;
            _nodes[i]._lineMaterial = _lineMaterial;
            _nodes[i]._speed = _speed * Random.Range(0.5f, 1.2f);
        }
    }

    /// <summary>
    /// start connect from the first node
    /// </summary>
    private void StartConnect()
    {
        _nodes[0]._lineMaterial = _lineMaterial;
        _nodes[0].Connect();
    }

    /// <summary>
    /// let nodes to find their neighbours
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Node[] GetConnectableNodes(Vector3 origin, float range)
    {
        List<Node> connectableNodes = new List<Node>();

        for(int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i].GetIsConnected()) { continue; }
            if(Vector3.Distance(origin, _nodes[i].transform.position) > range) { continue; }

            connectableNodes.Add(_nodes[i]);
        }

        return connectableNodes.ToArray();
    }

    #endregion

    #region Test on GPU

    /// <summary>
    /// initialize compute buffers
    /// </summary>
    private void InitializeBuffers()
    {
        _nodesBuffer = new ComputeBuffer(_nodeCount, Marshal.SizeOf(typeof(Node_ComputeShader)));
        // temporarily test with 3 times of node count
        _connectionBuffer = new ComputeBuffer(_nodeCount * 3, Marshal.SizeOf(typeof(Connection_ComputeShader)));
    }

    /// <summary>
    /// initialize parameters of compute shader
    /// </summary>
    private void InitializeParams()
    {
        __nodeConnectionControl.SetInt(_nodeCountParamName, _nodeCount);
    }

    /// <summary>
    /// release compute buffers
    /// </summary>
    private void ReleaseBuffers()
    {
        _nodesBuffer.Dispose();
        _connectionBuffer.Dispose();
    }


    /// <summary>
    /// generate nodes
    /// </summary>
    private void SpawnNodes_GPU()
    {
        _nodes = new Node[_nodeCount];
        Vector3 positionTemp = Vector3.zero;
        _nodesBufferData = new Node_ComputeShader[_nodeCount];
        for (int i = 0; i < _nodeCount; i++)
        {
            // instantiate node in the scene
            Transform newNode = Instantiate(_nodePrefab).transform;
            positionTemp.x = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
            positionTemp.y = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
            positionTemp.z = Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);
            newNode.position = positionTemp;
            newNode.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            newNode.SetParent(this.transform);
            _nodes[i] = newNode.GetComponent<Node>();
            _nodes[i]._nodesManager = this;
            _nodes[i]._lineMaterial = _lineMaterial;
            _nodes[i]._speed = _speed * Random.Range(0.5f, 1.2f);

            // set data for compute buffer
            _nodesBufferData[i]._id = i;
            _nodesBufferData[i]._position = newNode.position;
            _nodesBufferData[i]._rotation =  new Vector4(newNode.rotation.x, newNode.rotation.y, newNode.rotation.z, newNode.rotation.w);
            
        }

        _nodesBuffer.SetData(_nodesBufferData);
    }
    #endregion
}
