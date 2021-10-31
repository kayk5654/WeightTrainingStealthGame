using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// emit particles when a projecile dies, then destroy itself afterwards
/// </summary>
public class DeathParticlesController : MonoBehaviour
{

    [SerializeField, Tooltip("particles to emit when this projectile dies")]
    private VisualEffect _projectileDeathParticle;

    // duration between emission of particles and destroying this object
    private float _waitDuration = 1f;

    /// <summary>
    /// emit particles when this projectile dies
    /// </summary>
    public void EmitParticles()
    {
        StartCoroutine(EmitParticlesSequence());
    }

    /// <summary>
    /// process to emit projectile death particles
    /// </summary>
    /// <returns></returns>
    private IEnumerator EmitParticlesSequence()
    {
        _projectileDeathParticle.SendEvent("OnPlay");
        yield return new WaitForSeconds(_waitDuration);
        Destroy(this.gameObject);
    }
}
