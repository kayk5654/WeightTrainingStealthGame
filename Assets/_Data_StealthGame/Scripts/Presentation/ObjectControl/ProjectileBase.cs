﻿using System.Collections;
using UnityEngine;
/// <summary>
/// base class of projectiles to be spawn by items of player and enemy
/// </summary>
public class ProjectileBase : MonoBehaviour
{
    // identify projectile
    protected int _id;
    
    // move direction for calculation
    protected Vector3 _moveDirection;

    protected Vector3 _spawnPosition;

    // a coroutine to delete this projectile when it lost attack target
    protected IEnumerator _destroySelfSequence;

    protected RevealAreaHandler _revealAreaHandler;

    protected MaterialRevealHandler _materialRevealHandler;



    /// <summary>
    /// initialization
    /// </summary>
    protected virtual void Start()
    {

    }

    /// <summary>
    /// update transform of projectile
    /// </summary>
    protected virtual void Update()
    {
        // if attack target is loast and lost target sequence isn't started yet, start sequence
        StartLostTaretSeuqnce();

        // update transform of this projectile
        UpdateTransform();
    }

    /// <summary>
    /// detect collision on the scene object
    /// </summary>
    protected void OnTriggerEnter(Collider other)
    {
        // check whether the hit object is enemy or node
        IHitTarget hitTarget = other.GetComponent<IHitTarget>();
        if (hitTarget != null) 
        {
            hitTarget.OnHit(transform.position);
        }

        // destroy itself
        Destroy(this.gameObject);
    }

    /// <summary>
    /// initialize move direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    /// <summary>
    /// initialize spawn position
    /// </summary>
    /// <param name="position"></param>
    public void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    /// <summary>
    /// initialize reference of MaterialRevealHandler
    /// </summary>
    /// <param name="handler"></param>
    public void SetRevealAreaHandler(RevealAreaHandler handler)
    {
        _revealAreaHandler = handler;
    }

    public void SetMaterialRevealHandler(MaterialRevealHandler handler)
    {
        _materialRevealHandler = handler;
    }

    /// <summary>
    /// set id of this projectile
    /// </summary>
    /// <param name="id"></param>
    public void SetId(int id)
    {
        _id = id;
    }

    /// <summary>
    /// update transform of this projectile
    /// </summary>
    protected virtual void UpdateTransform()
    {
        // if lost target sequence is already started, never calculate inside of update
        if (_destroySelfSequence != null) { return; }

        this.transform.position += _moveDirection * ItemConfig._projectileSpeed;
        this.transform.rotation *= Quaternion.FromToRotation(this.transform.forward, _moveDirection);
    }

    /// <summary>
    /// if attack target is loast and lost target sequence isn't started yet, start sequence
    /// </summary>
    protected void StartLostTaretSeuqnce()
    {
        if (Vector3.Distance(_spawnPosition, transform.position) > ItemConfig._maxProjectileDistance && _destroySelfSequence == null)
        {
            _destroySelfSequence = DestroySelfSequence();
            StartCoroutine(_destroySelfSequence);
            return;
        }
    }

    /// <summary>
    /// process to delete this projectile when it lost attack target
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DestroySelfSequence()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        
        // duration to fly without attack target
        float duration = 5f;

        // phase of this sequence
        float phase = 0f;

        _moveDirection = this.transform.forward;

        // make this fly for a period defined by duration
        while (phase < 1f)
        {
            phase += Time.deltaTime / duration;
            this.transform.position += _moveDirection * ItemConfig._projectileSpeed;
            yield return wait;
        }

        Destroy(this.gameObject);
    }
}
