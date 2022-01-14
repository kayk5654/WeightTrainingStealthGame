using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control camera damage effect kicked by the nodes
/// </summary>
public class CameraDamageEffectHandler : MonoBehaviour
{
    [SerializeField, Tooltip("mesh renderer for the damage effect")]
    private MeshRenderer _meshRenderer;

    [SerializeField, Tooltip("nodes manager to set callback")]
    private NodesManager _nodesManager;

    [SerializeField, Tooltip("nodes manager to set callback")]
    private EnemiesManager _enemiesManager;

    // material of the renderer
    private Material _material;

    // property of the damage effect
    private string _damageEffectPhaseProperty = "_DamagePhase";

    // coroutine of the damage effect
    private IEnumerator _damageEffectSequence;

    // wait for end of frame for the coroutine
    private WaitForEndOfFrame _waitForEndOfFrame;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _material = _meshRenderer.material;
        _waitForEndOfFrame = new WaitForEndOfFrame();

        if (_nodesManager)
        {
            _nodesManager._onNodeAttacked += PlayDamageEffect;
        }

        if (_enemiesManager)
        {
            _enemiesManager._onEnemyAttackAfterPlay += PlayDamageEffect;
        }
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        if (_nodesManager)
        {
            _nodesManager._onNodeAttacked -= PlayDamageEffect;
        }

        if (_enemiesManager)
        {
            _enemiesManager._onEnemyAttackAfterPlay -= PlayDamageEffect;
        }
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// start playing damage efect
    /// </summary>
    public void PlayDamageEffect(object sender, InGameObjectEventArgs args)
    {
        if(_damageEffectSequence != null)
        {
            StopCoroutine(_damageEffectSequence);
            _damageEffectSequence = null;
        }

        _damageEffectSequence = DamageEffectSequence();
        StartCoroutine(_damageEffectSequence);
    }

    /// <summary>
    /// process of the damage effect
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageEffectSequence()
    {
        float phase = 0f;
        float duration = 0.3f;

        _material.SetFloat(_damageEffectPhaseProperty, 0f);

        while (phase < 1f)
        {
            phase += Time.deltaTime / duration;

            _material.SetFloat(_damageEffectPhaseProperty, phase);

            yield return _waitForEndOfFrame;
        }

        _material.SetFloat(_damageEffectPhaseProperty, 1f);
        _damageEffectSequence = null;
    }
}
