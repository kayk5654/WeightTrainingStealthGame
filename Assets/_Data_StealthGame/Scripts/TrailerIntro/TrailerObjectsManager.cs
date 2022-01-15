using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage enemies for capturing videos for the trailer
/// </summary>
public class TrailerObjectsManager : MonoBehaviour
{
    [SerializeField, Tooltip("enemies")]
    private TrailerEnemy[] _enemiesForTrailer;

    [SerializeField, Tooltip("node")]
    private TrailerNode _node;

    [SerializeField, Tooltip("damage effect")]
    private CameraDamageEffectHandler _damageEffect;

    [SerializeField, Tooltip("red vignette")]
    private AfterPlayDamageEffectHandler _redVignette;

    [SerializeField, Tooltip("player's transform")]
    private Transform _mainCamera;

    [SerializeField, Tooltip("distance threshold to change enemy's state")]
    private float _distThreshold = 1.7f;

    [SerializeField, Tooltip("animator")]
    private Animator _animator;

    // whether updating enemy's state is enabled
    private bool _isEnemyInteractionEnabled;

    // animation state to play trailer animation
    private string _playAnimationState = "Play";

    // animation state to reset objects
    private string _resetAnimationState = "Reset";

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        SetEnemyCallback();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        RemoveEnemyCallback();
    }

    private void Update()
    {
        SetEnemyStateByDistance();
    }

    /// <summary>
    /// play trailer animation from a button
    /// </summary>
    public void PlayAnimation()
    {
        _animator.SetTrigger(_playAnimationState);
    }

    /// <summary>
    /// reset trailer animation from a button
    /// </summary>
    public void ResetAnimation()
    {
        _animator.SetTrigger(_resetAnimationState);
    }

    /// <summary>
    /// initialize object
    /// </summary>
    public void Initialize()
    {
        _redVignette.RevertDamageEffect();
        ResetEnemies();
    }

    /// <summary>
    /// enable to update enemies' state
    /// </summary>
    public void StartEnemyInteraction()
    {
        _isEnemyInteractionEnabled = true;
    }

    /// <summary>
    /// disable to update enemies' state
    /// </summary>
    public void StopEnemyInteraction()
    {
        _isEnemyInteractionEnabled = false;
    }

    /// <summary>
    /// set enemy's state by the distance from the player
    /// </summary>
    private void SetEnemyStateByDistance()
    {
        if (!_isEnemyInteractionEnabled) { return; }

        foreach(TrailerEnemy enemy in _enemiesForTrailer)
        {
            if (Vector3.Distance(_mainCamera.position, enemy.transform.position) < _distThreshold)
            {
                enemy.Attack();
            }
        }
        
    }

    /// <summary>
    /// enable to call damage effect from enemies
    /// </summary>
    private void SetEnemyCallback()
    {
        foreach(TrailerEnemy enemy in _enemiesForTrailer)
        {
            enemy._onAttack += _damageEffect.PlayDamageEffect;
            enemy._onAttack += _redVignette.PlayDamageEffect;
        }
    }

    /// <summary>
    /// disable to call damage effect from enemies
    /// </summary>
    private void RemoveEnemyCallback()
    {
        foreach (TrailerEnemy enemy in _enemiesForTrailer)
        {
            enemy._onAttack -= _damageEffect.PlayDamageEffect;
            enemy._onAttack -= _redVignette.PlayDamageEffect;
        }
    }

    /// <summary>
    /// let the node destroy the enemies
    /// </summary>
    public void DestroyEnemies()
    {
        //_node.Attack();
        
        foreach (TrailerEnemy enemy in _enemiesForTrailer)
        {
            enemy.Destroy();
        }
    }

    /// <summary>
    /// reset enemies
    /// </summary>
    private void ResetEnemies()
    {
        foreach (TrailerEnemy enemy in _enemiesForTrailer)
        {
            enemy.ResetState();
        }
    }
}
