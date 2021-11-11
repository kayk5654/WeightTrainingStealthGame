﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// define behaviour of an enemy
/// </summary>
public class Enemy : MonoBehaviour
{
     // range to search neighbours to connect
    private float _range = 1f;

    // id of each nodes
    private int _id = -1;

    // speed to extend lines to connect
    private float _speed = 1f;

    [SerializeField, Tooltip("threshold of distance to the attack target")]
    private float _attackTargetDistThresh = 0.25f;

    [SerializeField, Tooltip("main mesh")]
    private MeshRenderer _mainMeshRenderer;

    [SerializeField, Tooltip("environment vfx mesh")]
    private MeshRenderer _environmentVfxRenderer;

    [SerializeField, Tooltip("enemy's animator")]
    private Animator _animator;

    // material of main mesh
    private Material _mainMaterial;

    // material of environment vfx 
    private Material _environmentVfxMaterial;

    // control enemy's animation
    private EnemyAnimationHandler _enemyAnimationHandler;

    // move enemy's transform
    private EnemyMover _enemyMover;

    // for corouines
    private WaitForEndOfFrame _waitForEndOfFrame;

    // current state of this enemy instance
    private EnemyState _currentState = EnemyState.Search;

    // temprarily contain the nearest attack target
    private Node _nearestTarget;

    // process of attacking player's object
    private IEnumerator _attackSequence;

    // whether an enemy' transform or state can be updated
    private bool _toUpdate;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _mainMaterial = _mainMeshRenderer.material;
        _environmentVfxMaterial = _environmentVfxRenderer.material;
        _waitForEndOfFrame = new WaitForEndOfFrame();
        _enemyAnimationHandler = new EnemyAnimationHandler(_animator);
        _enemyMover = new EnemyMover(_speed, transform);
    }

    /// <summary>
    /// update status of the enemy
    /// </summary>
    private void Update()
    {
        if (!_toUpdate) { return; }
        
        if(_currentState != EnemyState.Search){ return; }

        if (!_nearestTarget)
        {
            // if _nearestTarget is null, enemy moves forward 
            _enemyMover.Move(transform.position + transform.forward, Vector3.zero);
            return;
        }

        // move enemy to search attack target
        _enemyMover.Move(_nearestTarget.transform.position, Vector3.zero);

        // if this enemy gets sufficiently close to the attack target, start attack it
        if (Vector3.Distance(_nearestTarget.transform.position, transform.position) < _attackTargetDistThresh)
        {
            _currentState = EnemyState.Attack;
            StartAttack();
        }
    }

    /// <summary>
    /// initialize parameters
    /// </summary>
    /// <param name="enemiesManager"></param>
    /// <param name="id"></param>
    /// <param name="range"></param>
    /// <param name="speed"></param>
    /// <param name="nodesManager"></param>
    public void InitParams(int id, float range, float speed)
    {
        _id = id;
        _range = range;
        _speed = speed;
    }

    /// <summary>
    /// enable to get id 
    /// </summary>
    /// <returns></returns>
    public int GetId()
    {
        return _id;
    }

    /// <summary>
    /// enable/disable updating transform
    /// </summary>
    /// <param name="state"></param>
    public void EnableUpdate(bool state)
    {
        _toUpdate = state;
    }

    /// <summary>
    /// start attack behaviour
    /// </summary>
    private void StartAttack()
    {
        if(_attackSequence != null)
        {
            return;
        }

        _attackSequence = AttackSequence();
        StartCoroutine(_attackSequence);
    }

    /// <summary>
    /// process of attacking player's object
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackSequence()
    {
        _enemyAnimationHandler.SetAttack();

        yield return _waitForEndOfFrame;

        // giving damage on the attacked player's object
        yield return new WaitForSeconds(4f);

        // when the player's object is destroyed, end attack sequence
        _nearestTarget = null;
        _enemyAnimationHandler.SetSearch();
        _currentState = EnemyState.Search;
        _attackSequence = null;
    }

    /// <summary>
    /// set the nearest node from EnemiesManager
    /// </summary>
    /// <param name="node"></param>
    public void SetNearestNode(Node node)
    {
        _nearestTarget = node;
    }
}
