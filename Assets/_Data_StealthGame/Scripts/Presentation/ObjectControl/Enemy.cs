﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// define behaviour of an enemy
/// </summary>
public class Enemy : InGameObjectBase, IHitTarget
{
    // notify manager of this object that this object is destroyed
    public override event EventHandler<InGameObjectEventArgs> _onDestroyed;

    // range to search neighbours to connect
    private float _range = 1f;

    // id of each nodes
    private int _id = -1;

    // speed to extend lines to connect
    private float _speed = 1.5f;

    // external force to avoid other enemies and boundary
    private Vector3 _externalForce = Vector3.zero;

    [SerializeField, Tooltip("threshold of distance to the attack target")]
    private float _attackTargetDistThresh = 0.25f;

    [SerializeField, Tooltip("main mesh")]
    private Renderer _mainMeshRenderer;

    [SerializeField, Tooltip("environment vfx mesh")]
    private Renderer _environmentVfxRenderer;

    [SerializeField, Tooltip("enemy's animator")]
    private Animator _animator;

    [SerializeField, Tooltip("reveal enemy's material")]
    private MaterialRevealHandler _materialRevealHandler;

    [SerializeField, Tooltip("sound effect control")]
    private InGameObjectAudioHandler _audioHandler;

    [SerializeField, Tooltip("visual effect control")]
    private InGameObjectVfxHandler _vfxHandler;

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

    // whether this enemy is found by the player
    private bool _isFound;

    // event to notify EnemiesManager when this enemy is attacking after the gameplay
    public event EventHandler<InGameObjectEventArgs> _onAttackAfterPlay;


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
        InitParams(Config._enemyHp, Config._enemyAttack, Config._enemyDefense);
    }

    /// <summary>
    /// update status of the enemy
    /// </summary>
    private void Update()
    {
        if (!_toUpdate) { return; }
        
        switch (_currentState)
        {
            case EnemyState.Search:
                _enemyMover.SetLerpFactorType(true);

                if (!_nearestTarget)
                {
                    // if _nearestTarget is null, enemy moves forward 
                    _enemyMover.Move(transform.position + transform.forward, _externalForce);
                    return;
                }

                // move enemy to search attack target
                _enemyMover.Move(_nearestTarget.transform.position, _externalForce);

                // if this enemy gets sufficiently close to the attack target, start attack it
                if (Vector3.Distance(_nearestTarget.transform.position, transform.position) < _attackTargetDistThresh)
                {
                    _currentState = EnemyState.Attack;
                    Attack();
                }
                break;

            case EnemyState.Attack:
                _enemyMover.SetLerpFactorType(false);

                // move enemy to search attack target
                if (!_nearestTarget) { return; }
                transform.SetParent(_nearestTarget.transform);
                _enemyMover.Move(_nearestTarget.transform.position, Vector3.zero);
                break;

            default:
                break;
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
    protected override void Attack()
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
        _audioHandler.PlayAttackSfx(false);

        yield return _waitForEndOfFrame;

        // let the node notify that it's damaged
        if (_nearestTarget != null)
        {
            _nearestTarget.StartDamage();
        }

        // giving damage on the attacked player's object
        while (_nearestTarget != null && _nearestTarget.GetRemainedHP() > 0f)
        {
            _nearestTarget.SetAttackPosition(transform.position);
            _nearestTarget.Damage(_attack);
            yield return null;
        }

        // check whether _nearestTarget still exists
        if (_nearestTarget)
        {
            // before destroying the target object, unparent all enemies
            Enemy[] attackingEnemies = _nearestTarget.GetComponentsInChildren<Enemy>();
            foreach (Enemy enemy in attackingEnemies)
            {
                enemy.transform.SetParent(_nearestTarget.transform.parent);
            }
            // destroy the object
            _nearestTarget.Destroy();
        }

        _audioHandler.StopLoopSfx();

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

    /// <summary>
    /// get current state of this enemy
    /// </summary>
    /// <returns></returns>
    public EnemyState GetCurrentState()
    {
        return _currentState;
    }

    /// <summary>
    /// set external force to avoid other enemies and boundary
    /// </summary>
    /// <param name="force"></param>
    public void SetForce(Vector3 force)
    {
        _externalForce = force;
    }

    /// <summary>
    /// process to execute when this enemy is hit by a projectile
    /// </summary>
    /// <param name="hitPosition"></param>
    public void OnHit(Vector3 hitPosition)
    {
        Find(hitPosition);
    }

    /// <summary>
    /// whether this object can be hit by projectiles
    /// </summary>
    /// <returns></returns>
    public bool CanBeHit()
    {
        return !_isFound;
    }

    /// <summary>
    /// find and visualize this enemy when a projectile hits this
    /// </summary>
    public void Find(Vector3 revealOrigin)
    {
        // if this enemy is already found, do nothing
        if (_isFound) { return; }
        
        _isFound = true;

        // set "unsearchable" layer so that this enemy won't disturb projectile
        gameObject.layer = LayerMask.NameToLayer(Config._unsearchableLayerName);

        // make this enemy visible
        _materialRevealHandler.SetRevealOrigin(revealOrigin);
        _materialRevealHandler.Reveal();
    }

    /// <summary>
    /// apply damage on this object
    /// </summary>
    /// <param name="damagePerAction">damage of single attack action</param>
    public override void Damage(float damagePerAction)
    {
        // if this enemy isn't found, do nothing
        if (!_isFound) { return; }

        ApplyDamage_SingleAttack(damagePerAction);
        _audioHandler.PlayDamagedSfx(false);
    }

    /// <summary>
    /// destroy this enemy instance
    /// </summary>
    public override void Destroy()
    {
        // if this enemy isn't found, do nothing
        if (!_isFound) { return; }

        // play sound effect
        _audioHandler.PlayDestroyedSfx();
        
        // play visual effects
        _vfxHandler.PlayDestroyedVfx();

        // send notifycation
        InGameObjectEventArgs args = new InGameObjectEventArgs(_id);
        _onDestroyed?.Invoke(this, args);
        
        // clear callbacks
        _onDestroyed = null;

        // destroy this enemy object
        float delay = 0.3f;
        StartCoroutine(DelayedDestroySequence(delay));
    }

    /// <summary>
    /// destroy this enemy after delay
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DelayedDestroySequence(float delay)
    {
        yield return new WaitForSeconds(delay);

        // destroy this enemy object
        Destroy(gameObject);
    }

    /// <summary>
    /// check whether this enemy is found by the player
    /// </summary>
    /// <returns></returns>
    public bool GetIsFound()
    {
        return _isFound;
    }

    /// <summary>
    /// let this enemy attack the player agter gameplay in the case of "game over"
    /// </summary>
    public void AttackPlayer(Transform playerTransform)
    {
        StartCoroutine(AttackPlayerSequence(playerTransform));
    }

    /// <summary>
    /// let enemy attack the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackPlayerSequence(Transform playerTransform)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        bool isAttacking = false;
        float playerAttackDistThreshold = 0.55f;

        while (true)
        {
            // move enemy to search the player
            _enemyMover.Move(playerTransform.position, _externalForce);

            // if this enemy gets sufficiently close to the attack target, start attack it
            if (!isAttacking && Vector3.Distance(playerTransform.position, transform.position) < playerAttackDistThreshold)
            {
                _enemyAnimationHandler.SetAttack();
                _audioHandler.PlayAttackSfx(false);

                InGameObjectEventArgs args = new InGameObjectEventArgs(_id);
                _onAttackAfterPlay?.Invoke(this, args);
                isAttacking = true;
                break;
            }
            yield return wait;
        }
    }

    /// <summary>
    /// pause object's behaviour
    /// </summary>
    public override void Pause()
    {
        // pause attack process
        if(_attackSequence != null)
        {
            StopCoroutine(_attackSequence);
        }

        _enemyAnimationHandler.Pause();
    }

    /// <summary>
    /// resume object's behaviour
    /// </summary>
    public override void Resume()
    {
        // resume attack process
        if (_attackSequence != null)
        {
            StartCoroutine(_attackSequence);
        }

        _enemyAnimationHandler.Resume();
    }
}
