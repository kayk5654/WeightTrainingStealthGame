using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage enemies for capturing videos for the trailer
/// </summary>
public class TrailerObjectsManager : MonoBehaviour
{
    [SerializeField, Tooltip("enemies")]
    private Enemy[] _enemiesForTrailer;

    [SerializeField, Tooltip("node")]
    private Node _node;

    [SerializeField, Tooltip("damage effect")]
    private CameraDamageEffectHandler _damageEffect;

    [SerializeField, Tooltip("red vignette")]
    private AfterPlayDamageEffectHandler _redVignette;

    /// <summary>
    /// initialize enemies dictionary
    /// </summary>
    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
