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

    [SerializeField, Tooltip("max rarious of a single reveal area")]
    private float _maxRange = 0.8f;

    [SerializeField, Tooltip("reveal speed")]
    private float _speed = 0.1f;

    [SerializeField, Tooltip("material to control")]
    private Material[] _materials;

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

    // buffer index to set next reveal area
    private int _nextBufferIndex = 0;

    // store data of reveal areas to copy to the compute buffer
    private RevealArea[] _revealAreas;

    // coroutines to control reveal areas
    private List<IEnumerator> _revealCoroutines = new List<IEnumerator>();

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        // initialize reveal area buffer
        _revealAreaBuffer = new ComputeBuffer(_maxRevealAreaNum, Marshal.SizeOf(typeof(RevealArea)));
        
        RevealArea initRevealArea = new RevealArea()
        {
            _id = -1,
            _origin = Vector3.zero,
            _range = 0,
            _alpha = 1
        };

        _revealAreas = Enumerable.Repeat(initRevealArea, _maxRevealAreaNum).ToArray();

        _revealAreaBuffer.SetData(_revealAreas);

        // set max number of reveal area
        _computeShader.SetInt(ItemConfig._revealAreaNumName, _maxRevealAreaNum);
        Shader.SetGlobalInt(ItemConfig._revealAreaNumName, _maxRevealAreaNum);
        _calculateRevealAraeKernelIndex = _computeShader.FindKernel(_calculateRevealAreaKernelName);

        // set compute buffer on materials 
        foreach (Material material in _materials)
        {
            material.SetBuffer(_revealAreaBufferName, _revealAreaBuffer);
        }
    }

    /// <summary>
    /// execute calculation
    /// </summary>
    private void Update()
    {
        // debug
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetRevealAreas();
        }
        
        _revealAreaBuffer.SetData(_revealAreas);
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
    /// id is set by projectile
    /// </summary>
    public void Reveal(int id, Vector3 origin)
    {
        IEnumerator revealSequence = RevealSequence(id, origin);
        _revealCoroutines.Add(revealSequence);
        StartCoroutine(revealSequence);
    }

    /// <summary>
    /// revealing proecss
    /// </summary>
    /// <param name="maxRange"></param>
    /// <returns></returns>
    private IEnumerator RevealSequence(int id, Vector3 origin)
    {
        int bufferIndex = _nextBufferIndex;

        // set origin
        _revealAreas[bufferIndex]._id = id;
        _revealAreas[bufferIndex]._origin = origin;
        _nextBufferIndex++;

        // expand reveal area
        while(_revealAreas[bufferIndex]._range < _maxRange)
        {
            _revealAreas[bufferIndex]._range += Time.deltaTime * _speed;
            yield return waitForEndOfFrame;
        }

        _revealAreas[bufferIndex]._range = _maxRange;
    }

    /// <summary>
    /// reset all reveal areas
    /// </summary>
    public void ResetRevealAreas()
    {
        // stop all reveal coroutines
        for(int i = 0; i < _revealCoroutines.Count; i++)
        {
            StopCoroutine(_revealCoroutines[i]);
            _revealCoroutines[i] = null;
        }
        _revealCoroutines.Clear();

        // revert state of reveal areas
        for(int i = 0; i < _revealAreas.Length; i++)
        {
            _revealAreas[i]._id = -1;
            _revealAreas[i]._origin = Vector3.zero;
            _revealAreas[i]._range = 0;
            _revealAreas[i]._alpha = 1;
        }

        _nextBufferIndex = 0;

        // set reverted state on the buffer
        _revealAreaBuffer.SetData(_revealAreas);
        _computeShader.SetBuffer(_calculateRevealAraeKernelIndex, _revealAreaBufferName, _revealAreaBuffer);
    }
}
