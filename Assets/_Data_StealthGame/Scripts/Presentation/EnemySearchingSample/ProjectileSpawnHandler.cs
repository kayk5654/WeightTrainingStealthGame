﻿using System.Collections;
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

    [SerializeField, Tooltip("scene object reference")]
    private SceneObjectContainer _sceneObjectContainer;

    // get snapped cursor position
    private CursorSnapper _cursorSnapper;

    // id to set each projectiles
    private int _nextId = 0;

    // buffer to store target to shoot
    private Transform _tempTargetTransform;



    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _cursorSnapper = _sceneObjectContainer.GetCursorSnapper();
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

        // try to find target object
        _tempTargetTransform = _sceneObjectContainer.GetProjectileTargetFinder().GetTargetObject();

        if (_tempTargetTransform)
        {
            // if the target object is avaiable, set target on the projectile
            projectile.SetMoveTarget(_tempTargetTransform);
        }
        else
        {
           // if the target is unavailable, set move direction
            projectile.SetMoveDirection(_cursorSnapper.GetSnappedCursorPosition() - _spawnGuide.position);
        }
        
        projectile.SetSpawnPosition(_spawnGuide.position);
        
        _nextId++;
    }
}
