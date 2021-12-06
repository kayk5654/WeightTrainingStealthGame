using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// control visual effect graph for in-game objects
/// </summary>
public class InGameObjectVfxHandler : MonoBehaviour
{
    [SerializeField, Tooltip("attack vfx")]
    private VisualEffect _attackVfx;

    [SerializeField, Tooltip("duration of attack vfx")]
    private float _attackVfxDuration = 1f;

    [SerializeField, Tooltip("damaged vfx")]
    private VisualEffect _damagedVfx;

    [SerializeField, Tooltip("duration of damaged vfx")]
    private float _damagedVfxDuration = 1f;

    [SerializeField, Tooltip("destroyed vfx")]
    private VisualEffect _destroyedVfx;

    [SerializeField, Tooltip("duration of destroyed vfx")]
    private float _destroyedVfxDuration = 1f;


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
    /// play vfx for attacking
    /// </summary>
    public void PlayAttackVfx()
    {
        if (!_attackVfx) { return; }
        StartCoroutine(PlayAttackVfxSequence());
    }

    private IEnumerator PlayAttackVfxSequence()
    {
        _attackVfx.gameObject.SetActive(true);
        yield return new WaitForSeconds(_attackVfxDuration);
        _attackVfx.gameObject.SetActive(false);
    }

    /// <summary>
    /// play vfx for being damaged
    /// </summary>
    public void PlayDamagedVfx()
    {
        if (!_damagedVfx) { return; }
    }

    /// <summary>
    /// play vfx for being destroyed
    /// </summary>
    public void PlayDestroyedVfx()
    {
        if (!_destroyedVfx) { return; }
    }
}
