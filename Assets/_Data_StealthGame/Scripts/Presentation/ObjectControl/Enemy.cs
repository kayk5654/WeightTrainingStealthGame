using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// define behaviour of an enemy
/// </summary>
public class Enemy : MonoBehaviour
{
    // reference of NodesManager
    public EnemiesManager _nodesManager;

    [Tooltip("range to search neighbours to connect")]
    public float _range = 1f;

    [Tooltip("id of each nodes")]
    public int _id = -1;

    // speed to extend lines to connect
    public float _speed = 1f;

    [SerializeField, Tooltip("main mesh")]
    private MeshRenderer _mainMeshRenderer;

    [SerializeField, Tooltip("environment vfx mesh")]
    private MeshRenderer _environmentVfxRenderer;

    // material of main mesh
    private Material _mainMaterial;

    // material of environment vfx 
    private Material _environmentVfxMaterial;

    // for corouines
    WaitForEndOfFrame _waitForEndOfFrame;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _mainMaterial = _mainMeshRenderer.material;
        _environmentVfxMaterial = _environmentVfxRenderer.material;
        _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    /// <summary>
    /// update status of the enemy
    /// </summary>
    private void Update()
    {
        
    }
}
