using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// spawn projectile to search object 
/// for testing purpose
/// </summary>
public class ProjectileSpawnHandler : MonoBehaviour
{
    [SerializeField, Tooltip("test projectile prefab")]
    private ProjectileBase _projectile;

    [SerializeField, Tooltip("guide transform to spawn projectiles")]
    private Transform _spawnGuide;

    [SerializeField, Tooltip("manage in-game input")]
    private InGameInputManager _inGameInputManager;

    [SerializeField, Tooltip("scene material control")]
    private RevealAreaHandler _revealAreaHandler;

    [SerializeField, Tooltip("scene material control without compute buffer")]
    private MaterialRevealHandler _materialRevealHandler;

    // id to set each projectiles
    private int _nextId = 0;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _inGameInputManager._onPush += Spawn;
    }

    /// <summary>
    /// disable callback
    /// </summary>
    private void OnDisable()
    {
        _inGameInputManager._onPush -= Spawn;
    }

    /// <summary>
    /// spawn projectile by any action
    /// it'll be called as a callback of input
    /// </summary>
    private void Spawn(object sender, EventArgs e)
    {
        ProjectileBase projectile = Instantiate(_projectile, _spawnGuide.position, _spawnGuide.rotation, null);
        projectile.SetId(_nextId);
        projectile.SetMoveDirection(_spawnGuide.forward);
        projectile.SetSpawnPosition(_spawnGuide.position);
        if (_revealAreaHandler)
        {
            projectile.SetRevealAreaHandler(_revealAreaHandler);
        }
        
        if (_materialRevealHandler)
        {
            projectile.SetMaterialRevealHandler(_materialRevealHandler);
        }
        _nextId++;
    }
}
