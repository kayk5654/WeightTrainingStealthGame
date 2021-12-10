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

    [SerializeField, Tooltip("damaged vfx")]
    private VisualEffect _damagedVfx;

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
        _attackVfx.Play();
    }

    /// <summary>
    /// play vfx for being damaged
    /// </summary>
    public void PlayDamagedVfx()
    {
        if (!_damagedVfx) { return; }
        _damagedVfx.Play();
    }

    /// <summary>
    /// play vfx for being destroyed
    /// </summary>
    public void PlayDestroyedVfx()
    {
        if (!_destroyedVfx) { return; }
        StartCoroutine(DestroyedVfxSequence());
    }

    /// <summary>
    /// process of playing destroyed vfx
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyedVfxSequence()
    {
        // unparent vfx from the node
        transform.SetParent(null);

        // play vfx
        _destroyedVfx.Play();
        yield return new WaitForSeconds(_destroyedVfxDuration);

        // destroy gameobject
        Destroy(gameObject);
    }
}
