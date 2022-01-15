using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// spawn nodes using NodesManager for the trailer
/// </summary>
public class TrailerNodeSpawnHandler : MonoBehaviour
{
    [SerializeField, Tooltip("nodes manager")]
    private NodesManager _nodesManager;

    [SerializeField, Tooltip("number of node")]
    private int _nodeCount = 20;

    private PlayerAbilityDataSet _playerAbilityDataset;

    private void Start()
    {
        _playerAbilityDataset = new PlayerAbilityDataSet();
        _playerAbilityDataset._unlockedNodeNumber = _nodeCount;
        Spawn();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// spawn nodes
    /// </summary>
    public void Spawn()
    {
        _nodesManager.Spawn(_playerAbilityDataset);
    }

    /// <summary>
    /// delete nodes
    /// </summary>
    public void Delete()
    {
        _nodesManager.Delete();
    }
}
