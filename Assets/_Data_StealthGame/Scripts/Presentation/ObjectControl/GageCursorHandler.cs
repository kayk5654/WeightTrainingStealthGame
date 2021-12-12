using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control gage-cursor object
/// </summary>
public class GageCursorHandler : MonoBehaviour
{
    [SerializeField, Tooltip("root transform")]
    private Transform _root;

    [SerializeField, Tooltip("gage transform")]
    private MeshRenderer _gage;

    [SerializeField, Tooltip("cursor plane mesh renderer")]
    private MeshRenderer _cursorPlane;

    [SerializeField, Tooltip("animator for the gage and the cursor")]
    private Animator _animator;

    [SerializeField, Tooltip("sound effect audio source")]
    private AudioSource _sfxSource;

    [SerializeField, Tooltip("snap sound effect")]
    private AudioClip _snapSfxClip;

    [SerializeField, Tooltip("charge sound effect")]
    private AudioSource _chargeSfxSource;

    // material for the cursor plane
    private Material _cursorMaterial;

    // material for the ring gage
    private Material _gageMaterial;

    // property name of the displayed angle of the ring gage
    private string _gageAngleProperty = "_Angle";

    // animator property to scale the cursor up
    private string _scaleUpProperty = "ScaleUp";

    // animator property to scale the cursor down
    private string _scaleDownProperty = "ScaleDown";

    // animator property to initialize the cursor size
    private string _initScaleProperty = "Init";


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _cursorMaterial = _cursorPlane.material;
        _gageMaterial = _gage.material;
    }

    /// <summary>
    /// scale the cursor up
    /// </summary>
    public void ScaleUp()
    {
        _animator.SetTrigger(_scaleUpProperty);
        _sfxSource.PlayOneShot(_snapSfxClip);
    }

    /// <summary>
    /// scale the cursor down
    /// </summary>
    public void ScaleDown()
    {
        _animator.SetTrigger(_scaleDownProperty);
    }

    /// <summary>
    /// initialize the size of the cursor
    /// </summary>
    public void InitScale()
    {
        _animator.SetTrigger(_initScaleProperty);
    }

    /// <summary>
    /// set filled angle of the ring gage
    /// </summary>
    /// <param name="angle"></param>
    public void SetRingGageAngle(float angle)
    {
        _gageMaterial.SetFloat(_gageAngleProperty, angle);

        if (angle > 0f && angle < 1f && !_chargeSfxSource.isPlaying)
        {
            _chargeSfxSource.Play();
        }
        else if (angle <= 0f || angle >= 1f)
        {
            _chargeSfxSource.Stop();
        }
    }
}
