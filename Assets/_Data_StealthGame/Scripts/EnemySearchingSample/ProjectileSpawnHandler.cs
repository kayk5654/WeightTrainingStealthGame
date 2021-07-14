using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// spawn projectile to search object 
/// for testing purpose
/// </summary>
public class ProjectileSpawnHandler : MonoBehaviour
{
    [SerializeField, Tooltip("test projectile prefab")]
    private ProjectileBase _projectile;

    [SerializeField, Tooltip("guide transform to spawn projectiles (optional)")]
    private Transform _spawnGuide;


    private void Start()
    {
        
    }

    /// <summary>
    /// spawn projectile by any action
    /// it'll be called as a callback of input
    /// </summary>
    private void Spawn()
    {
        ProjectileBase projectile = Instantiate(_projectile, _spawnGuide.position, _spawnGuide.rotation, null);
        projectile.SetMoveDirection(_spawnGuide.forward);
        projectile.SetSpawnPosition(_spawnGuide.position);
    }
}
