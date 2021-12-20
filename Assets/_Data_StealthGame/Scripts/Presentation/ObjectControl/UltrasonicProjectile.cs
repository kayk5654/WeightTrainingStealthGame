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
    protected GameObject _deathParticlesPrefab;

    [SerializeField, Tooltip("particle system")]
    private ParticleSystem _mainParticle;

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
    protected override void OnHit()
    {
        _deathParticlesController = Instantiate(_deathParticlesPrefab, transform.position, transform.rotation).GetComponent<DeathParticlesController>();
        _deathParticlesController.EmitParticles();
        _mainParticle.transform.SetParent(_deathParticlesController.transform);
        _mainParticle.Stop();
        base.OnHit();
    }

    /// <summary>
    /// play sound effect on spawn
    /// </summary>
    private void PlaySpawnSfx()
    {
        _sfxSource.PlayOneShot(_spawnSfx);
    }
}
