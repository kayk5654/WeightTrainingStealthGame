using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control ultrasonic projectile pbject
/// </summary>
public class UltrasonicProjectile : ProjectileBase
{
    [SerializeField, Tooltip("audio source for ound effects")]
    private AudioSource _sfxSource;

    [SerializeField, Tooltip("sound effect for spawn")]
    private AudioClip _spawnSfx;

    [SerializeField, Tooltip("particles to emit when this projectile dies")]
    protected DeathParticlesController _deathParticlesController;

    /// <summary>
    /// initialization
    /// </summary>
    protected override void Start()
    {
        PlaySpawnSfx();
    }

    /// <summary>
    /// emit particles when this projectile dies
    /// </summary>
    private void OnDestroy()
    {
        _deathParticlesController.transform.parent = null;
        _deathParticlesController.gameObject.SetActive(true);
        _deathParticlesController.EmitParticles();
    }

    /// <summary>
    /// play sound effect on spawn
    /// </summary>
    private void PlaySpawnSfx()
    {
        _sfxSource.PlayOneShot(_spawnSfx);
    }
}
