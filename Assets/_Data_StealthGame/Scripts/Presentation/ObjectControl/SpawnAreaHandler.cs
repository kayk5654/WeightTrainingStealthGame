using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage spawn area
/// enable to get spawn area depending on the exercise type selection
/// </summary>
public class SpawnAreaHandler : MonoBehaviour
{
    // center of the spawn area
    private Vector3 _center;

    // size of the spawn area
    private Vector3 _size;

    // world-space rotation of the spawn area
    private Quaternion _rotation;

    private Vector3 _boundLocalMin;
    private Vector3 _boundLocalMax;

    /// <summary>
    /// overrde spawn area
    /// </summary>
    public void GetSpawnArea(out Vector3 center, out Vector3 size, out Quaternion rotation)
    {
        center = _center;
        size = _size;
        rotation = _rotation;
    }
}
