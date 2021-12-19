using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage events called from an animation clip for "Player's mission" phase of the tutorial
/// </summary>
public class TutorialMissionAnimEventController : MonoBehaviour
{
    [SerializeField, Tooltip("enemy's animator")]
    private Animator _enemyAnimator;

    [SerializeField, Tooltip("audio source for the sound effects")]
    private AudioSource _sfxSource;

    [SerializeField, Tooltip("enemy's attack sfx")]
    private AudioClip _enemyAttackSfx;

    [SerializeField, Tooltip("node's destroyed sfx")]
    private AudioClip _nodeDestroyedSfx;

    private string _enemyAnimProperty = "IsAttack";


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// set enemy's animation "searching"
    /// </summary>
    public void SetEnemySearchAnimation()
    {
        _enemyAnimator.SetBool(_enemyAnimProperty, false);
    }

    /// <summary>
    /// set enemy's animation "attack"
    /// </summary>
    public void SetEnemyAttackAnimation()
    {
        _enemyAnimator.SetBool(_enemyAnimProperty, true);
    }

    /// <summary>
    /// play _enemyAttackSfx
    /// </summary>
    public void PlayEnemyAttackSfx()
    {
        _sfxSource.PlayOneShot(_enemyAttackSfx);
    }

    /// <summary>
    /// play _nodeDestroyedSfx
    /// </summary>
    public void PlayNodeDestroyedSfx()
    {
        _sfxSource.PlayOneShot(_nodeDestroyedSfx);
    }
}
