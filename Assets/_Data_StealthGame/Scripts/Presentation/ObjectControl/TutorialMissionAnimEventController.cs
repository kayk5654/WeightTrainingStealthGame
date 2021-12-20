using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
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

    [SerializeField, Tooltip("vfx of sphere nodes")]
    private VisualEffect[] _nodeVfxHandlers;

    [SerializeField, Tooltip("renderer of sphere nodes")]
    private Renderer[] _nodeRenderers;

    // property to switch enemy's animation
    private string _enemyAnimProperty = "IsAttack";

    // property to control damage effect of the nodes
    private string _nodeDamageProperty = "_DamageRange";

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
    /// reset state of the objects
    /// </summary>
    public void ResetState()
    {
        _enemyAnimator.SetBool(_enemyAnimProperty, false);

        for (int i = 0; i < _nodeRenderers.Length; i++) 
        {
            if (_nodeRenderers[i].material.HasProperty(_nodeDamageProperty))
            {
                _nodeRenderers[i].material.SetFloat(_nodeDamageProperty, 0f);
            }
            
            _nodeRenderers[i].enabled = true;
        }
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
    public void PlayEnemyAttackSfx(int targetNodeId)
    {
        _sfxSource.PlayOneShot(_enemyAttackSfx);

        if (_nodeRenderers[targetNodeId].material.HasProperty(_nodeDamageProperty))
        {
            _nodeRenderers[targetNodeId].material.SetFloat(_nodeDamageProperty, 1f);
        }
    }

    /// <summary>
    /// destroy the selected node
    /// </summary>
    /// <param name="nodeIndex"></param>
    public void DestroyNode(int nodeIndex)
    {
        _nodeVfxHandlers[nodeIndex].Play();
        _sfxSource.PlayOneShot(_nodeDestroyedSfx);
        _nodeRenderers[nodeIndex].enabled = false;
    }
}
