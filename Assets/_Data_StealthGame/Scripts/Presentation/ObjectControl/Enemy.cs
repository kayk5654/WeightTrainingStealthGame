using System.Collections;
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
    private float _speed = 1f;

    // external force to avoid other enemies and boundary
    private Vector3 _externalForce = Vector3.zero;

    [SerializeField, Tooltip("threshold of distance to the attack target")]
    private float _attackTargetDistThresh = 0.25f;

    [SerializeField, Tooltip("main mesh")]
    private MeshRenderer _mainMeshRenderer;

    [SerializeField, Tooltip("environment vfx mesh")]
    private MeshRenderer _environmentVfxRenderer;

    [SerializeField, Tooltip("enemy's animator")]
    private Animator _animator;

    [SerializeField, Tooltip("reveal enemy's material")]
    private MaterialRevealHandler _materialRevealHandler;

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

        yield return _waitForEndOfFrame;

        // giving damage on the attacked player's object
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);

        // temporarily disable destroying player's object for testing purpose
        /*
        while (true)
        {
            yield return null;
        }
        */
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

    }

    /// <summary>
    /// destroy this enemy instance
    /// </summary>
    public override void Destroy()
    {
        // if this enemy isn't found, do nothing
        if (!_isFound) { return; }

        // send notifycation
        InGameObjectEventArgs args = new InGameObjectEventArgs(_id);
        _onDestroyed?.Invoke(this, args);
        
        // clear callbacks
        _onDestroyed = null;

        // destroy this enemy object
        Destroy(gameObject);
    }
}
