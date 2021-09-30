using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// contain parameters of node
/// </summary>
public class Node : MonoBehaviour
{
    // reference of NodesManager
    public NodesManager _nodesManager;

    [Tooltip("range to search neighbours to connect")]
    public float _range = 1f;

    [Tooltip("id of each nodes")]
    public int _id;

    // whether this node is already connected to others
    private bool _isConencted;

    // lines to connect to other nodes
    private LineRenderer[] _lines;

    // speed to extend lines to connect
    public float _speed = 1f;

    // width of lines to connect
    private float _lineWidth = 0.006f;

    // material of lines
    public Material _lineMaterial;

    [SerializeField, Tooltip("node mesh")]
    private MeshRenderer _mainMeshRenderer;

    // material of node mesh
    private Material _nodeMaterial;

    [SerializeField, Tooltip("node cap")]
    private List<Transform> _nodeCaps = new List<Transform>();

    // for corouines
    WaitForEndOfFrame _waitForEndOfFrame;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _nodeMaterial = _mainMeshRenderer.material;
        WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        _nodeCaps[0].gameObject.SetActive(false);
    }

    /// <summary>
    /// connect this node to another node
    /// </summary>
    public void Connect()
    {
        _isConencted = true;
        _mainMeshRenderer.enabled = true;
        StartCoroutine(ConnectSequence());
    }

    /// <summary>
    /// connection process to other nodes
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConnectSequence()
    {
        // search connectable nodes
        Node[] connectableNodes = _nodesManager.GetConnectableNodes(transform.position, _range);

        float phase = 0f;
        _lines = new LineRenderer[connectableNodes.Length];
        for (int i = 0; i < _lines.Length; i++)
        {
            // initialize connection lines
            LineRenderer lineObject = new GameObject().AddComponent<LineRenderer>();
            lineObject.transform.SetParent(transform);
            lineObject.transform.localPosition = Vector3.zero;
            lineObject.transform.localRotation = Quaternion.identity;
            _lines[i] = lineObject;
            _lines[i].useWorldSpace = false;
            _lines[i].SetPosition(0, Vector3.zero);
            _lines[i].SetPosition(1, Vector3.zero);
            _lines[i].material = _lineMaterial;
            _lines[i].startWidth = _lineWidth;
            _lines[i].endWidth = _lineWidth;
            _lines[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // duplicate node caps for each connections
            if (i > 0)
            {
                Transform newNodeCap = Instantiate(_nodeCaps[0].gameObject, _nodeCaps[0].parent).transform;
                newNodeCap.LookAt(connectableNodes[i].transform);
                _nodeCaps.Add(newNodeCap);
                newNodeCap.gameObject.SetActive(true);
            }
            else
            {
                _nodeCaps[0].transform.LookAt(connectableNodes[0].transform);
                _nodeCaps[0].gameObject.SetActive(true);
            }
            
        }

        // connect nodes
        IEnumerator[] connectionSequences = new IEnumerator[connectableNodes.Length];

        for(int i = 0; i < connectableNodes.Length; i++)
        {
            connectionSequences[i] = SingleConnectionSequence(_lines[i], connectableNodes[i], _speed * Random.Range(0.5f, 1.5f));
            StartCoroutine(connectionSequences[i]);
        }

        yield return null;
    }

    /// <summary>
    /// connect to another node by extending a line
    /// </summary>
    /// <param name="line"></param>
    /// <param name="node"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private IEnumerator SingleConnectionSequence(LineRenderer line, Node node, float speed)
    {
        node.SpawnNodeCapForReceiving(transform);
        
        float phase = 0f;

        while (phase < 1.0f)
        {
            phase += Time.deltaTime * speed;

            line.SetPosition(1, Vector3.Lerp(Vector3.zero, transform.InverseTransformPoint(node.transform.position) * transform.localScale.x, phase));
            yield return _waitForEndOfFrame;
        }

        line.SetPosition(1, transform.InverseTransformPoint(node.transform.position) * transform.localScale.x);
        node.Connect();
    }

    /// <summary>
    /// get whether this node is connected to others
    /// </summary>
    /// <returns></returns>
    public bool GetIsConnected()
    {
        return _isConencted;
    }

    /// <summary>
    /// spawn node cap of this node from another node
    /// </summary>
    public void SpawnNodeCapForReceiving(Transform lookAtTarget)
    {
        Transform newNodeCap = Instantiate(_nodeCaps[0].gameObject, _nodeCaps[0].parent).transform;
        newNodeCap.LookAt(lookAtTarget);
        newNodeCap.gameObject.SetActive(true);
        _nodeCaps.Add(newNodeCap);
    }

    /// <summary>
    /// set simulated result on the NodeConnectionControl.compute
    /// </summary>
    public void SetSimulatedData(Node_ComputeShader nodeData)
    {
        if(nodeData._id != _id) { return; }
        //Debug.Log("nodeID: " + _id + " / updated position: " + nodeData._position);
        transform.position = nodeData._position;
        transform.rotation = new Quaternion(nodeData._rotation.x, nodeData._rotation.y, nodeData._rotation.z, nodeData._rotation.w);

    }
}
