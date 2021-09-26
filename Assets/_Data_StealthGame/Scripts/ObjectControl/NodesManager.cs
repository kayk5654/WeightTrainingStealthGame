using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manages connected nodes
/// </summary>
public class NodesManager : MonoBehaviour
{
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


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        SpawnNodes();
        StartConnect();
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
    }

    private void Update()
    {
        
    }

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
}
