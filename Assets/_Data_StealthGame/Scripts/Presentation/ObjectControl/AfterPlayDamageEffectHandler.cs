using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control camera damage effect kicked by the enemies after the gameplay
/// </summary>
public class AfterPlayDamageEffectHandler : MonoBehaviour
{
    [SerializeField, Tooltip("mesh renderer for the damage effect")]
    private MeshRenderer _meshRenderer;

    [SerializeField, Tooltip("nodes manager to set callback")]
    private EnemiesManager _enemiesManager;

    // material of the renderer
    private Material _material;

    // property of the red vignette
    private string _redVignetteProperty = "_Vignette";

    // wait for end of frame for the coroutine
    private WaitForEndOfFrame _waitForEndOfFrame;

    // whether the red vignette is turned on
    private bool _isRedVignetteActivated;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _material = _meshRenderer.material;
        _waitForEndOfFrame = new WaitForEndOfFrame();
        _enemiesManager._onEnemyAttackAfterPlay += PlayDamageEffect;
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _enemiesManager._onEnemyAttackAfterPlay -= PlayDamageEffect;
        
    }

    /// <summary>
    /// hide red vignette
    /// </summary>
    private void OnDisable()
    {
        RevertDamageEffect();
    }

    private void Update()
    {

    }

    /// <summary>
    /// start playing damage efect
    /// </summary>
    private void PlayDamageEffect(object sender, InGameObjectEventArgs args)
    {
        if (!_isRedVignetteActivated)
        {
            StartCoroutine(RedVignetteSequence());
            _isRedVignetteActivated = true;
        }
    }

    /// <summary>
    /// process of displaying red vignette
    /// </summary>
    /// <returns></returns>
    private IEnumerator RedVignetteSequence()
    {
        float phase = 0f;
        float duration = 0.3f;

        _material.SetFloat(_redVignetteProperty, 0f);

        while (phase < 1f)
        {
            phase += Time.deltaTime / duration;

            _material.SetFloat(_redVignetteProperty, phase);

            yield return _waitForEndOfFrame;
        }

        _material.SetFloat(_redVignetteProperty, 1f);
    }

    /// <summary>
    /// revert condition of the damage effect
    /// </summary>
    private void RevertDamageEffect()
    {
        _isRedVignetteActivated = false;
        _material.SetFloat(_redVignetteProperty, 0f);
    }
}
