using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control audio feedback of the in-game objects
/// this script should be attached on the independent gameobject,
/// which is originally a child of the root object of an in-game object
/// </summary>
public class InGameObjectAudioHandler : MonoBehaviour
{
    [SerializeField, Tooltip("audio source to control sound effects")]
    private AudioSource _sfxSource;

    [SerializeField, Tooltip("attack sound effect")]
    private AudioClip _attackSfx;

    [SerializeField, Tooltip("damaged sound effect")]
    private AudioClip _damagedSfx;

    [SerializeField, Tooltip("destroyed sound effect")]
    private AudioClip _destroyedSfx;


    /// <summary>
    /// play attack sfx
    /// </summary>
    /// <param name="isLoop"></param>
    public void PlayAttackSfx(bool isLoop)
    {
        if (isLoop)
        {
            if (!_sfxSource.isPlaying || _sfxSource.clip != _attackSfx)
            {
                _sfxSource.clip = _attackSfx;
                _sfxSource.Play();
            }
        }
        else
        {
            _sfxSource.PlayOneShot(_attackSfx);
        }
    }

    /// <summary>
    /// play damaged sfx
    /// </summary>
    /// <param name="isLoop"></param>
    public void PlayDamagedSfx(bool isLoop)
    {
        if (isLoop)
        {
            if (!_sfxSource.isPlaying || _sfxSource.clip != _damagedSfx)
            {
                _sfxSource.clip = _damagedSfx;
                _sfxSource.Play();
            }        }
        else
        {
            _sfxSource.PlayOneShot(_damagedSfx);
        }
    }

    /// <summary>
    /// stop looping sound effect
    /// </summary>
    public void StopLoopSfx()
    {
        _sfxSource.Stop();
    }

    /// <summary>
    /// play destroyed sfx
    /// </summary>
    public void PlayDestroyedSfx()
    {
        StartCoroutine(PlayDestroyedSfxSequence());
    }

    /// <summary>
    /// process to play destroyed sfx
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayDestroyedSfxSequence()
    {
        // unparent gameobject
        transform.parent = null;

        // stop looping sound
        StopLoopSfx();

        // play sound effect
        _sfxSource.PlayOneShot(_destroyedSfx);
        yield return new WaitForSeconds(_destroyedSfx.length);

        // destroy this game object
        Destroy(gameObject);
    }
}
