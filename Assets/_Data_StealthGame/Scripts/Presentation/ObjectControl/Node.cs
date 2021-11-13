using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// contain parameters of node
/// </summary>
public class Node : InGameObjectBase, IHitTarget
{
    // reference of NodesManager
    private NodesManager _nodesManager;

    [Tooltip("id of each nodes")]
    private int _id = -1;

    // speed to extend lines to connect
    private float _speed = 1f;

    [SerializeField, Tooltip("range of attacking enemies nearby")]
    private float _attackRadius = 0.4f;

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

    // event to notify nodeManager when this node is destroyed
    public override event EventHandler<InGameObjectEventArgs> _onDestroyed;

    // store connected node temporarily
    private Node _connectedNodeTemp;


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
            _connectedNodeTemp = _nodesManager.GetNode(anotherNodeId);
            if (_connectedNodeTemp)
            {
                SpawnNodeCap(_connectedNodeTemp.transform);
            }
            
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
            Connection connectionTemp = _nodesManager.GetConnection(_connectionsIds[i]);
            if (!connectionTemp) 
            {
                // remove node cap
                Transform removeNodecap = _nodeCaps[i];
                _nodeCaps.Remove(removeNodecap);
                Destroy(removeNodecap.gameObject);
                i--;
                continue;
            }

            int anotherNodeId = connectionTemp.GetAnotherNode(_id);
            _connectedNodeTemp = _nodesManager.GetNode(anotherNodeId);
            
            if (!_connectedNodeTemp) { continue; }
            _nodeCaps[i].LookAt(_connectedNodeTemp?.transform);
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

    /// <summary>
    /// reaction of this node when it is hit by a projectile
    /// </summary>
    /// <param name="hitPosition"></param>
    public void OnHit(Vector3 hitPosition)
    {
        // attack nearby enemies
        Attack();

        // notify the order to attack to the other nodes connected with it
    }

    // attack enemies nearby this node
    private void Attack()
    {
        // search enemies nearby this node
        Enemy[] enemies = FindEnemies();

        if(enemies == null || enemies.Length < 1) { return; }

        // apply damage in the final implementation

        // destroy enemies immediately for testing
        foreach(Enemy enemy in enemies)
        {
            enemy.Destroy();
        }
    }

    /// <summary>
    /// find enemies nearby this node
    /// </summary>
    /// <returns></returns>
    private Enemy[] FindEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        
        // find nearby colliders
        Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius);

        if(colliders.Length < 1) { return null; }

        // check whether the colliders are enemies
        Enemy enemyTemp;
        foreach(Collider collider in colliders)
        {
            enemyTemp = collider.GetComponent<Enemy>();
            if (enemyTemp == null) { continue; }
            enemies.Add(enemyTemp);
        }

        return enemies.ToArray();
    }

    /// <summary>
    /// process when this node is destroyed by an enemy
    /// </summary>
    public override void Destroy()
    {
        InGameObjectEventArgs args = new InGameObjectEventArgs(_id);
        _onDestroyed?.Invoke(this, args);

        // remove all callbacks before destroying
        _onDestroyed = null;

        Destroy(gameObject);
    }

    /// <summary>
    /// remove registered connection if the relative connection is destroyed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnDestroyConnection(object sender, InGameObjectEventArgs args)
    {
        if (!_connectionsIds.Contains(args._id)) { return; }
        _connectionsIds.Remove(args._id);
    }
}
