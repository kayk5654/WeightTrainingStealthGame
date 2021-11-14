using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// spawn projectile to search object 
/// for testing purpose
/// </summary>
public class ProjectileSpawnHandler : MonoBehaviour, IInGameOffenseAction
{
    [SerializeField, Tooltip("test projectile prefab")]
    private ProjectileBase _projectile;

    [SerializeField, Tooltip("guide transform to spawn projectiles")]
    private Transform _spawnGuide;

    // id to set each projectiles
    private int _nextId = 0;



    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// disable callback
    /// </summary>
    private void OnDisable()
    {
        
    }

    /// <summary>
    /// do offense action
    /// </summary>
    public void Attack()
    {
        Spawn();
    }

    /// <summary>
    /// spawn projectile by any action
    /// it'll be called as a callback of input
    /// </summary>
    private void Spawn()
    {
        ProjectileBase projectile = Instantiate(_projectile, _spawnGuide.position, _spawnGuide.rotation, null);
        projectile.SetId(_nextId);
        projectile.SetMoveDirection(_spawnGuide.forward);
        projectile.SetSpawnPosition(_spawnGuide.position);
        
        _nextId++;
    }
}
