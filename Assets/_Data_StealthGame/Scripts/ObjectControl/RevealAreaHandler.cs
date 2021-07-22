using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class RevealAreaHandler : MonoBehaviour
{
    [SerializeField, Tooltip("calculation and management of reveal area")]
    private ComputeShader _computeShader;

    [SerializeField, Tooltip("max number of reveal area")]
    private int _maxRevealAreaNum = 256;

    // structure of single revealing area
    public struct RevealArea
    {
        int _id; // identify areas from c# scripts
        Vector3 _origin; // origin of the reveal area
        float _range; // current radious of the reveal area
        float _alpha; // phase of fading out of the reveal area
    }

    // buffer for revealing area
    private ComputeBuffer _revealAreaBuffer;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _revealAreaBuffer = new ComputeBuffer(_maxRevealAreaNum, Marshal.SizeOf(typeof(RevealArea)));
    }

    private void Update()
    {
        
    }
}
