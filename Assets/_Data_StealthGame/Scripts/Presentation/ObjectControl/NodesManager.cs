using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

/// <summary>
/// data of single node
/// </summary>
public struct Node_ComputeShader
{
    // identify each nodes
    public int _id;
    // position of a node
    public Vector3 _position;
    // velocity of a node
    public Vector3 _velocity;
}

/// <summary>
/// data of connection between nodes
/// </summary>
public struct Connection_ComputeShader
{
    // identify each connections
    public int _id;
    // node to connect
    public int _connectNode1;
    // node to connect
    public int _connectNode2;
}

/// <summary>
/// manages connected nodes
/// </summary>
public class NodesManager : MonoBehaviour, IItemManager<PlayerAbilityDataSet, SpawnAreaDataSet>
{
    [SerializeField, Tooltip("compute shader for node control")]
    private ComputeShader _nodeConnectionControl;

    [SerializeField, Tooltip("node to instantiate")]
    private GameObject _nodePrefab;

    [SerializeField, Tooltip("connection to instantiate")]
    private Connection _connectionPrefab;

    [SerializeField, Tooltip("area to spawn nodes")]
    private BoxCollider _spawnArea;

    [SerializeField, Tooltip("number of nodes")]
    private int _nodeCount = 30;

    [SerializeField, Tooltip("max number of connection between nodes")]
    private int _maxConnectionPerNode = 3;

    // dictionary of _node instances
    private Dictionary<int, Node> _nodes;

    // dictionary of connection instances
    private Dictionary<int, Connection> _connections;

    [SerializeField, Tooltip("material of line")]
    private Material _lineMaterial;

    [SerializeField, Tooltip("connecting speed")]
    private float _speed = 1f;

    [SerializeField, Tooltip("range of neighbour nodes which affects single node's behaviour")]
    private float _neighbourRadious = 0.4f;

    [SerializeField, Tooltip("range of searching connectable nodes fron a specific node")]
    private float _connectRadious = 0.55f;

    [SerializeField, Tooltip("weight of the velocity to avoid boundary")]
    private float _avoidBoundaryVelWeight = 1f;

    [SerializeField, Tooltip("spawn enemy objects in the spawn area")]
    private ObjectSpawnHandler _objectSpawnHandler;

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

    // parameter name of _maxConnectionPerNode
    private string _maxConnectionPerNodeName = "_maxConnectionPerNode";

    // parameter name of _connectRadious
    private string _connectRadiousName = "_connectRadious";

    // parameter name of _floorHeight
    private string _floorHeightName = "_floorHeight";

    // parameter name of _avoidBoundaryVelWeight
    private string _avoidBoundaryVelWeightName = "_avoidBoundaryVelWeight";

    // parameter name of _boundaryCenter
    private string _boundaryCenterName = "_boundaryCenter";

    // parameter name of _boundarySize
    private string _boundarySizeName = "_boundarySize";

    // parameter name of _boundaryRotation
    private string _boundaryRotationName = "_boundaryRotation";

    // name of node buffer on compute shader for reading
    private string _nodeBufferName_Read = "_nodesBufferRead";

    // name of node buffer on compute shader for writing
    private string _nodeBufferName_Write = "_nodesBufferWrite";

    // name of connection buffer on compute shader for reading
    private string _connectionBufferName_Read = "_connectionBufferRead";

    // name of connection buffer on compute shader for writing
    private string _connectionBufferName_Write = "_connectionBufferWrite";

    // kernel name of UpdateNode()
    private string _updateNodeKernelName = "UpdateNode";

    // kernel name of InitializeConnection()
    private string _initConnectionKernelName = "InitializeConnection";

    // kernel name of FindNearestNode()
    private string _findNearestNodeKernelName = "FindNearestNode";

    // kernel info of UpdateNodePosition()
    private KernelParamsHandler _updateNodePosKernel;

    // kernel info of InitializeConnection()
    private KernelParamsHandler _initConnectionKernel;

    // kernel info of FindNearestNode()
    private KernelParamsHandler _findNearestNodKernel;
    // index of buffers for reading
    private const int READ = 0;

    // index of buffers for writing
    private const int WRITE = 1;

    // thread size of a thread group
    private const int SIMULATION_BLOCK_SIZE = 256;

    // whether the nodes must be updated in this frame
    private bool _toUpdate = false;


    #region MonoBehaviour
    private void Update()
    {
        if (!_toUpdate) { return; }
        
        SimulateNodes_GPU();
        //SimulateConnections_GPU();
        UpdatePositioniForConnection();
    }

    #endregion

    #region IItemManagerBase

    /// <summary>
    /// spawn scene objects
    /// </summary>
    /// <param name="dataset"></param>
    public void Spawn(PlayerAbilityDataSet dataset, SpawnAreaDataSet spawnArea = null)
    {
        LoadDataSet(dataset);
        _objectSpawnHandler.SetSpawnArea(spawnArea);

        InitializeBuffers();
        InitializeParams();
        SpawnNodes_GPU();
        SpawnConnection_GPU();

        _toUpdate = true;
    }

    /// <summary>
    /// pause update of scene objects
    /// </summary>
    public void Pause()
    {
        _toUpdate = false;
    }

    /// <summary>
    /// resume update of scene objects
    /// </summary>
    public void Resume()
    {
        _toUpdate = true;
    }

    /// <summary>
    /// delete scene objects when the gameplay ends
    /// </summary>
    public void Delete()
    {
        _toUpdate = false;

        foreach (Node node in _nodes.Values)
        {
            Destroy(node.gameObject);
        }
        _nodes.Clear();

        foreach (Connection connection in _connections.Values)
        {
            Destroy(connection.gameObject);
        }
        _connections.Clear();

        // release buffers
        ReleaseBuffers();
    }

    #endregion

    #region NodeInfoAccessors

    /// <summary>
    /// return a node specified by its id
    /// </summary>
    /// <param name="nodeId"></param>
    /// <returns></returns>
    public Node GetNode(int nodeId)
    {
        return _nodes[nodeId];
    }

    /// <summary>
    /// return a connection specified by its id
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public Connection GetConnection(int connectionId)
    {
        return _connections[connectionId];
    }

    #endregion

    #region Handle ComputeShader

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
            _connectionBuffers[i] = new ComputeBuffer(_maxConnectionPerNode * _nodeCount, Marshal.SizeOf(typeof(Connection_ComputeShader)));
        }
    }

    /// <summary>
    /// initialize parameters of compute shader
    /// </summary>
    private void InitializeParams()
    {
        // calculate thread group size
        int nodeKernelThreadGroupSize = Mathf.CeilToInt((float)_nodeCount / (float) SIMULATION_BLOCK_SIZE);
        int connectionKernelThreadGroupSize = Mathf.CeilToInt((float) (_maxConnectionPerNode * _nodeCount) / (float) SIMULATION_BLOCK_SIZE);

        // contain data of kernel in KernelParamsHandler
        _updateNodePosKernel = new KernelParamsHandler(_nodeConnectionControl, _updateNodeKernelName, nodeKernelThreadGroupSize, 1, 1);
        _initConnectionKernel = new KernelParamsHandler(_nodeConnectionControl, _initConnectionKernelName, connectionKernelThreadGroupSize, 1, 1);
        _findNearestNodKernel = new KernelParamsHandler(_nodeConnectionControl, _findNearestNodeKernelName, nodeKernelThreadGroupSize, 1, 1);

        // set constant parameters for simulation
        _nodeConnectionControl.SetFloat(_neighbourRadiousName, _neighbourRadious);
        _nodeConnectionControl.SetInt(_nodeCountName, _nodeCount);
        _nodeConnectionControl.SetInt(_maxConnectionPerNodeName, _maxConnectionPerNode);
        _nodeConnectionControl.SetFloat(_connectRadiousName, _connectRadious);
        _nodeConnectionControl.SetFloat(_avoidBoundaryVelWeightName, _avoidBoundaryVelWeight);
        _nodeConnectionControl.SetVector(_boundaryCenterName, _spawnArea.transform.TransformPoint(_spawnArea.center));
        _nodeConnectionControl.SetVector(_boundarySizeName, _spawnArea.size);
        _nodeConnectionControl.SetMatrix(_boundaryRotationName, Matrix4x4.TRS(_spawnArea.transform.position, _spawnArea.transform.rotation, _spawnArea.transform.lossyScale));
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
        _nodes = new Dictionary<int, Node>();
        Vector3 positionTemp = Vector3.zero;
        _nodesBufferData = new Node_ComputeShader[_nodeCount];

        Vector3 boundLocalMin = _spawnArea.center - _spawnArea.size * 0.5f;
        Vector3 boundLocalMax = _spawnArea.center + _spawnArea.size * 0.5f;

        for (int i = 0; i < _nodeCount; i++)
        {
            // instantiate node in the scene
            Transform newNode = _objectSpawnHandler.Spawn(_nodePrefab, this.transform).transform;

            _nodes.Add(i, newNode.GetComponent<Node>());
            _nodes[i].InitParams(i, this, _speed * Random.Range(0.5f, 1.2f));

            // set data for compute buffer
            _nodesBufferData[i]._id = i;
            _nodesBufferData[i]._position = newNode.position;
            _nodesBufferData[i]._velocity = Vector3.zero;


        }

        // contain data of nodes in the compute buffer
        _nodesBuffers[READ].SetData(_nodesBufferData);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Read, _nodesBuffers[READ]);

    }

    /// <summary>
    /// generate connection between nodes
    /// </summary>
    private void SpawnConnection_GPU()
    {
        // initialize list and array
        _connections = new Dictionary<int, Connection>();
        _connectionBufferData = new Connection_ComputeShader[(_maxConnectionPerNode * _nodeCount)];

        Connection_ComputeShader initConnection = new Connection_ComputeShader()
        {
            _id = -1,
            _connectNode1 = -1,
            _connectNode2 = -1
        };

        _connectionBufferData = Enumerable.Repeat(initConnection, (_maxConnectionPerNode * _nodeCount)).ToArray();

        // calculate initial connection between nodes on the compute shader
        _connectionBuffers[READ].SetData(_connectionBufferData);
        _nodeConnectionControl.SetBuffer(_initConnectionKernel._index, _connectionBufferName_Read, _connectionBuffers[READ]);
        // _nodeBufferName_Read is set in SpawnNodes_GPU()
        _nodeConnectionControl.SetBuffer(_initConnectionKernel._index, _nodeBufferName_Read, _nodesBuffers[READ]);
        _nodeConnectionControl.SetBuffer(_initConnectionKernel._index, _connectionBufferName_Write, _connectionBuffers[WRITE]);
        _nodeConnectionControl.Dispatch(_initConnectionKernel._index, _initConnectionKernel._x, _initConnectionKernel._y, _initConnectionKernel._z);

        // get result of calculation
        _connectionBuffers[WRITE].GetData(_connectionBufferData);

        // apply result of calculation
        for (int i = 0; i < _connectionBufferData.Length; i++)
        {
            // skip if _id of connection data is still -1; it doesn't contain info of connection
            if(_connectionBufferData[i]._id == -1) { continue; }

            // instantiate connection
            Connection newConnection = Instantiate<Connection>(_connectionPrefab, transform);
            newConnection._id = _connectionBufferData[i]._id;
            newConnection.SetNodeIds(_connectionBufferData[i]._connectNode1, _connectionBufferData[i]._connectNode2);
            newConnection.SetNodesPosition(_nodes[_connectionBufferData[i]._connectNode1].transform.position, _nodes[_connectionBufferData[i]._connectNode2].transform.position);

            bool isKeyAlreadyExist = _connections.ContainsKey(newConnection._id);
            if (isKeyAlreadyExist)
            {
                Debug.LogError("key " + newConnection._id + " already exists!");
                continue;
            }
            _connections.Add(newConnection._id, newConnection);

            // let the relative nodes know about this connection
            _nodes[_connectionBufferData[i]._connectNode1].AddConnection(newConnection._id);
            _nodes[_connectionBufferData[i]._connectNode2].AddConnection(newConnection._id);
        }

        // spawn node caps for each connections
        foreach(Node node in _nodes.Values)
        {
            node.InitializeNodeCaps();
        }

        // swap buffers
        SwapBuffers(_connectionBuffers);
    }

    /// <summary>
    /// simulate nodes' behaviour
    /// </summary>
    private void SimulateNodes_GPU()
    {
        // caulculate nodes' behaviour in the compute shader
        _nodeConnectionControl.SetFloat(_deltatimeName, Time.deltaTime);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Read, _nodesBuffers[READ]);
        _nodeConnectionControl.SetBuffer(_updateNodePosKernel._index, _nodeBufferName_Write, _nodesBuffers[WRITE]);
        _nodeConnectionControl.Dispatch(_updateNodePosKernel._index, _updateNodePosKernel._x, _updateNodePosKernel._y, _updateNodePosKernel._z);

        // update nodes in the scene
        _nodesBuffers[WRITE].GetData(_nodesBufferData);
        foreach(int key in _nodes.Keys)
        {
            _nodes[key].SetSimulatedData(_nodesBufferData[key]);
        }

        // swap buffers
        SwapBuffers(_nodesBuffers);
    }

    /// <summary>
    /// update connection between nodes
    /// </summary>
    private void SimulateConnections_GPU()
    {
        // temporarily keep initial connection
        // change of connection (remove, create new connection) will be considered later

        
    }

    /// <summary>
    /// update positions of nodes used for connection while keep initial connection
    /// </summary>
    private void UpdatePositioniForConnection()
    {
        // as long as keeping initial connection,
        // the index of _connectionBufferData always matches the index of _connections

        // connection data to access
        Connection_ComputeShader connectionData;

        // apply update of position of nodes
        foreach(int key in _connections.Keys)
        {
            // key of _connections matches index of _connectionBufferData,
            // since the unused slot of _connectionBufferData contains the data with _id = -1
            if(key >= _connectionBufferData.Length || key < 0)
            {
                Debug.LogError("key " + key + " is out of range of _connectionBufferData");
                continue;
            }
            
            connectionData = _connectionBufferData[key];
            _connections[key].SetNodesPosition(_nodes[connectionData._connectNode1].transform.position, _nodes[connectionData._connectNode2].transform.position);
        }
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

    /// <summary>
    /// get the nearest node from the specified point
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public Node GetNearestNode(Vector3 origin)
    {
        return _nodes[0];
    }
    #endregion

    #region Other Methods

    /// <summary>
    /// apply loaded level data info to this class
    /// </summary>
    /// <param name="dataSet"></param>
    private void LoadDataSet(PlayerAbilityDataSet dataSet)
    {
        _nodeCount = dataSet._unlockedNodeNumber;
    }

    #endregion
}
