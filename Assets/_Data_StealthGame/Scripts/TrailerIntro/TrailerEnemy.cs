using System.Collections;
using System;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// control enemy's behaviour for the trailer
/// </summary>
public class TrailerEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("sound effect control")]
    private AudioSource _destroyAudio;

    [SerializeField, Tooltip("sound effect control for attacking")]
    private AudioSource _attackAudio;

    [SerializeField, Tooltip("visual effect control")]
    private VisualEffect _destroyVfx;

    [SerializeField, Tooltip("enemy's animator")]
    private Animator _animator;

    [SerializeField, Tooltip("mesh renderer")]
    private Renderer[] _renderers;

    // callback when attacking something
    public event EventHandler<InGameObjectEventArgs> _onAttack;

    // whether this enemy is "attacking" state
    private bool _isAttacking;

    // animator property
    private string _attackAnimationProperty = "IsAttack";


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// set enemy's state "attack"
    /// </summary>
    public void Attack()
    {
        if (_isAttacking) { return; }

        _animator.SetBool(_attackAnimationProperty, true);
        _attackAudio.Play();
        InGameObjectEventArgs args = new InGameObjectEventArgs(0);
        _onAttack?.Invoke(this, args);
        _isAttacking = true;
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

        _animator.SetBool(_attackAnimationProperty, false);

        _isAttacking = false;
    }
}
