using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
/// <summary>
/// pass values for calculation of reveal area on RevealAreaControl.compute
/// </summary>
public class RevealAreaHandler : MonoBehaviour
{
    [SerializeField, Tooltip("calculation and management of reveal area")]
    private ComputeShader _computeShader;

    [SerializeField, Tooltip("max number of reveal area")]
    private int _maxRevealAreaNum = 256;

    // kernel name of CalculateRevealArea() on RevealAreaControl.compute
    private string _calculateRevealAreaKernelName = "CalculateRevealArea";

    // kernel index of CalculateRevealArea() on RevealAreaControl.compute
    private int _calculateRevealAraeKernelIndex;

    // buffer name of _revealAreaBuffer on RevealAreaControl.compute
    private string _revealAreaBufferName = "_revealAreaBuffer";

    // structure of single revealing area
    public struct RevealArea
    {
        public int _id; // identify areas from c# scripts
        public Vector3 _origin; // origin of the reveal area
        public float _range; // current radious of the reveal area
        public float _alpha; // phase of fading out of the reveal area
    }

    // buffer for revealing area
    private ComputeBuffer _revealAreaBuffer;

    // wait for end of frame in the coroutine
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        // initialize reveal area buffer
        _revealAreaBuffer = new ComputeBuffer(_maxRevealAreaNum, Marshal.SizeOf(typeof(RevealArea)));
        
        RevealArea initRevealArea = new RevealArea()
        {
            _id = 0,
            _origin = Vector3.zero,
            _range = 0,
            _alpha = 1
        };

        _revealAreaBuffer.SetData(Enumerable.Repeat(initRevealArea, _maxRevealAreaNum).ToArray());

        // set max number of reveal area
        _computeShader.SetInt(ItemConfig._revealAreaNumName, _maxRevealAreaNum);
        Shader.SetGlobalInt(ItemConfig._revealAreaNumName, _maxRevealAreaNum);
        _calculateRevealAraeKernelIndex = _computeShader.FindKernel(_calculateRevealAreaKernelName);
    }

    /// <summary>
    /// execute calculation
    /// </summary>
    private void Update()
    {
        _computeShader.SetBuffer(_calculateRevealAraeKernelIndex, _revealAreaBufferName, _revealAreaBuffer);
        _computeShader.Dispatch(_calculateRevealAraeKernelIndex, _maxRevealAreaNum, 1, 1);
    }

    /// <summary>
    /// release buffer
    /// </summary>
    private void OnDestroy()
    {
        _revealAreaBuffer.Release();
    }

    /// <summary>
    /// reveal material
    /// </summary>
    private void Reveal()
    {
        StartCoroutine(RevealSequence(3.0f));
    }

    /// <summary>
    ///  set origin of the reveal area 
    /// </summary>
    /// <param name="origin"></param>
    private void SetRevealOrigin(Vector3 origin)
    {
        Vector4 tempVector;
        /*
        foreach (Material mat in _materials)
        {
            tempVector = mat.GetVector(_revealAreaProperty);
            tempVector.x = origin.x;
            tempVector.y = origin.y;
            tempVector.z = origin.z;
            mat.SetVector(_revealAreaProperty, tempVector);
        }*/
    }

    /// <summary>
    /// revealing proecss
    /// </summary>
    /// <param name="maxRange"></param>
    /// <returns></returns>
    private IEnumerator RevealSequence(float maxRange)
    {
        /*
        Vector4 tempVector = _materials[0].GetVector(_revealAreaProperty);

        while (tempVector.w < maxRange)
        {
            foreach (Material mat in _materials)
            {
                //tempVector = mat.GetVector(_revealAreaProperty);
                //tempVector.w += Time.deltaTime * _speed;
                //mat.SetVector(_revealAreaProperty, tempVector);
            }

            yield return waitForEndOfFrame;
        }
        */
        yield return waitForEndOfFrame;
    }
}
