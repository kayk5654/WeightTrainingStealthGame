using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control ring gage to start workout
/// </summary>
public class RingGageHandler : MonoBehaviour
{
    [SerializeField, Tooltip("gage transform")]
    private MeshRenderer _gage;

    [SerializeField, Tooltip("audio source for sound effects")]
    private AudioSource _sfxSource;

    [SerializeField, Tooltip("look time checker to start gameplay")]
    private StayStillTimeChecker _timeCountChecker;

    // material for the ring gage
    private Material _gageMaterial;

    // property name of the displayed angle of the ring gage
    private string _gageAngleProperty = "_Angle";

    [Tooltip("enable updating ring gage filling")]
    public bool _enableUpdating = true;


    /// <summary>
    /// initialize
    /// </summary>
    private void Start()
    {
        _gageMaterial = _gage.material;
    }

    private void OnEnable()
    {
        if (_gageMaterial)
        {
            _gageMaterial.SetFloat(_gageAngleProperty, 0f);
        }
    }

    private void OnDisable()
    {
        _sfxSource.Stop();
    }

    /// <summary>
    /// update filling status of the ring gage
    /// </summary>
    private void Update()
    {
        if (!_enableUpdating) { return; }
        
        SetRingGageAngle(_timeCountChecker.GetNormalizedTime());
    }

    /// <summary>
    /// set filled angle of the ring gage
    /// </summary>
    /// <param name="angle"></param>
    private void SetRingGageAngle(float angle)
    {
        _gageMaterial.SetFloat(_gageAngleProperty, angle);

        if(angle == 0f)
        {
            _sfxSource.Stop();
        }
        else if (!_sfxSource.isPlaying)
        {
            _sfxSource.Play();
        }
    }
}
