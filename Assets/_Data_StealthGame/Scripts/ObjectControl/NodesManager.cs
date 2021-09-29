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
    private ComputeShader _nodeConnectionControl;

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

    [SerializeField, Tooltip("range of neighbour nodes which affects single node's behaviour")]
    private float _neighbourRadious = 0.4f;

    // buffer for nodes
    private ComputeBuffer[] _nodesBuffers;

    // buffer for connections
    private ComputeBuffer[] _connectionBuffers;

    // data to set _nodesBuffer
    private Node_ComputeShader[] _nodesBufferData;

    // data to set _connectionBuffer
    private Connection_ComputeShader[] _connectionBufferData;

    // parameter name of _nodeCount
    private string _nodeCountName = "_nodeCount";

    // parameter name of _neighbourRadious
    private string _neighbourRadiousName = "_neighbourRadious";

    // parameter name of _deltaTime
    private string _deltatimeName = "_deltaTime";

    // name of node buffer on compute shader for reading
    private string _nodeBufferName_Read = "_nodesBufferRead";

    // name of node buffer on compute shader for writing
    private string _nodeBufferName_Write = "_nodesBufferWrite";

    // name of connection buffer on compute shader for reading
    private string _connectionBufferName_Read = "_connectionBufferRead";

    // name of connection buffer on compute shader for writing
    private string _connectionBufferName_Write = "_connectionBufferWrite";

    // kernel name of UpdateNodePosition()
    private string _updateNodePosKernelName = "UpdateNodePosition";

    // kernel info of UpdateNodePosition()
    private KernelParamsHandler _updateNodePosKernel;

    // index of buffers for reading
    private const int READ = 0;

    // index of buffers for writing
    private const int WRITE = 1;

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
    /// delete nodes
    /// </summary>
    private void OnDestroy()
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
        SimulateNodes_GPU();
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
        // create 2 compute buffers for each purpose to avoid conflict 
        _nodesBuffers = new ComputeBuffer[2];
        _connectionBuffers = new ComputeBuffer[2];

        for(int i = 0; i < _nodesBuffers.Length; i++)
        {
            _nodesBuffers[i] = new ComputeBuffer(_nodeCount, Marshal.SizeOf(typeof(Node_ComputeShader)));
            // temporarily test with 3 times of node count
            _connectionBuffers[i] = new ComputeBuffer(_nodeCount * 3, Marshal.SizeOf(typeof(Connection_ComputeShader)));
        }
    }

    /// <summary>
    /// initialize parameters of compute shader
    /// </summary>
    private void InitializeParams()
    {
        // contain data of kernel in KernelParamsHandler
        _updateNodePosKernel = new KernelParamsHandler(_nodeConnectionControl, _updateNodePosKernelName, _nodeCount, 1, 1);

        // set constant parameters for simulation
        _nodeConnectionControl.SetFloat(_neighbourRadiousName, _neighbourRadious);
        _nodeConnectionControl.SetInt(_nodeCountName, _nodeCount);
    }

    /// <summary>
    /// release compute buffers
    /// </summary>
    private void ReleaseBuffers()
    {
        for (int i = 0; i < _nodesBuffers.Length; i++)
        {
            _nodesBuffers[i].Release();
            _connectionBuffers[i].Release();
        }
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

        // contain data of nodes in the compute buffer
        _nodesBuffers[READ].SetData(_nodesBufferData);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Read, _nodesBuffers[READ]);

        // empty buffer data array?
        _nodesBufferData = null;

    }

    /// <summary>
    /// simulate nodes' behaviour
    /// </summary>
    private void SimulateNodes_GPU()
    {
        _nodeConnectionControl.SetFloat(_deltatimeName, Time.deltaTime);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Read, _nodesBuffers[READ]);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Write, _nodesBuffers[WRITE]);
        _nodeConnectionControl.Dispatch(_updateNodePosKernel._index, _updateNodePosKernel._x, _updateNodePosKernel._y, _updateNodePosKernel._z);

        // update nodes in the scene


        // swap buffers
        SwapBuffers(_nodesBuffers);
    }

    /// <summary>
    /// swap compute buffers for reading and writing
    /// </summary>
    /// <param name="buffers"></param>
    private void SwapBuffers(ComputeBuffer[] buffers)
    {
        ComputeBuffer temp = buffers[READ];
        buffers[READ] = buffers[WRITE];
        buffers[WRITE] = temp;
    }
    #endregion
}
