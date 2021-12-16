using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// emit particles when a projecile dies, then destroy itself afterwards
/// </summary>
public class DeathParticlesController : MonoBehaviour
{

    [SerializeField, Tooltip("particles to emit when this projectile dies")]
    private ParticleSystem _projectileDeathParticle;

    [SerializeField, Tooltip("hit sound effect audio source")]
    private AudioSource _hitSfxSource;

    [SerializeField, Tooltip("hit sound effect")]
    private AudioClip _hitSfxClip;


    // duration between emission of particles and destroying this object
    private float _waitDuration = 2f;

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
        _projectileDeathParticle.Play();
        _hitSfxSource.PlayOneShot(_hitSfxClip);
        yield return new WaitForSeconds(_waitDuration);
        Debug.Log("death particle emittion");
        Destroy(this.gameObject);
    }
}
