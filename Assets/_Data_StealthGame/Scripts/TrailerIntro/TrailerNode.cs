using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// control node's behaviour for the trailer
/// </summary>
public class TrailerNode : MonoBehaviour
{
    [SerializeField, Tooltip("attack vfx")]
    private VisualEffect _attackVfx;

    [SerializeField, Tooltip("attack sfx")]
    private AudioSource _attackSfx;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// play attack vfx and sfx
    /// </summary>
    public void Attack()
    {
        _attackVfx.Play();
        _attackSfx.Play();
    }
}
