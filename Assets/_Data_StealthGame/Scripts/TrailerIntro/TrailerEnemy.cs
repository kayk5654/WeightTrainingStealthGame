using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// control enemy's behaviour for the trailer
/// </summary>
public class TrailerEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("sound effect control")]
    private AudioSource _destroyAudio;

    [SerializeField, Tooltip("visual effect control")]
    private VisualEffect _destroyVfx;

    [SerializeField, Tooltip("enemy's animator")]
    private Animator _animator;

    [SerializeField, Tooltip("mesh renderer")]
    private Renderer[] _renderers;

    // control enemy's animation
    private EnemyAnimationHandler _enemyAnimationHandler;

    private void Start()
    {
        _enemyAnimationHandler = new EnemyAnimationHandler(_animator);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// set enemy's state "attack"
    /// </summary>
    public void Attack()
    {
        _enemyAnimationHandler.SetAttack();
    }

    /// <summary>
    /// play destroy effects
    /// </summary>
    public void Destroy()
    {
        _destroyVfx.Play();
        _destroyAudio.Play();
        foreach(Renderer renderer in _renderers)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// reset state of the enemy
    /// </summary>
    public void ResetState()
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.enabled = true;
        }

        _enemyAnimationHandler.SetSearch();
    }
}
