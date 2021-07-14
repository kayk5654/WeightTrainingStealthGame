using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control revaling material for testing perpose
/// </summary>
public class MaterialRevealHandler : MonoBehaviour
{
    [SerializeField, Tooltip("material to control")]
    private Material[] _materials;

    [SerializeField, Tooltip("reveal speed")]
    private float _speed;

    // _revealOrigin should be contained in a static class in the future development
    [Tooltip("origin of reveal area")]
    public Vector3 _revealOrigin;

    // property to control
    private string _revealAreaProperty = "_RevealArea";

    // coroutine for revealing process
    private IEnumerator _revealSequence;

    // wait for end of frame in the coroutine
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        ResetMaterial();
    }

    /// <summary>
    /// debugging
    /// </summary>
    private void Update()
    {
        // debug on the editor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetMaterial();
            SetRevealOrigin(_revealOrigin);
            Reveal();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetMaterial();
        }
    }

    /// <summary>
    /// debuging on pressable button for hololens
    /// </summary>
    public void TestRevealing()
    {
        ResetMaterial();
        SetRevealOrigin(_revealOrigin);
        Reveal();
    }

    /// <summary>
    ///  set origin of the reveal area
    /// </summary>
    /// <param name="origin"></param>
    private void SetRevealOrigin(Vector3 origin)
    {
        Vector4 tempVector;
        
        foreach(Material mat in _materials)
        {
            tempVector = mat.GetVector(_revealAreaProperty);
            tempVector.x = origin.x;
            tempVector.y = origin.y;
            tempVector.z = origin.z;
            mat.SetVector(_revealAreaProperty, tempVector);
        }
    }

    /// <summary>
    /// reveal material
    /// </summary>
    private void Reveal()
    {
        if (_revealSequence != null)
        {
            StopCoroutine(_revealSequence);
            _revealSequence = null;
        }

        _revealSequence = RevealSequence(3.0f);
        StartCoroutine(_revealSequence);
    }

    private IEnumerator RevealSequence(float maxRange)
    {
        Vector4 tempVector = _materials[0].GetVector(_revealAreaProperty);

        while (tempVector.w < maxRange)
        {
            foreach (Material mat in _materials)
            {
                tempVector = mat.GetVector(_revealAreaProperty);
                tempVector.w += Time.deltaTime * _speed;
                mat.SetVector(_revealAreaProperty, tempVector);
            }

            yield return waitForEndOfFrame;
        }

        _revealSequence = null;
    }

    /// <summary>
    /// reset material
    /// </summary>
    private void ResetMaterial()
    {
        if(_revealSequence != null)
        {
            StopCoroutine(_revealSequence);
            _revealSequence = null;
        }

        foreach (Material mat in _materials)
        {
            mat.SetVector(_revealAreaProperty, Vector4.zero);
        }
    }
}
