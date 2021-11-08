﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// provide a feature to spawn scene object
/// </summary>
public class ObjectSpawnHandler : MonoBehaviour
{
    [SerializeField, Tooltip("box collider to define a spawn area")]
    private BoxCollider _spawnArea;

    // max position of a bounding box in local space of the spawn area
    private Vector3 _boundLocalMin;

    // min position of a bounding box in local space of the spawn area
    private Vector3 _boundLocalMax;

    // variable for calculation to store position temporarily
    private Vector3 _positionTemp;

    /// <summary>
    /// calculate min and max corner of the bounding box of the spawn area
    /// </summary>
    /// <param name="spawnAreaData"></param>
    public void SetSpawnArea(SpawnAreaDataSet spawnAreaData)
    {
        if(spawnAreaData == null)
        {
            // calculate min and max corner from current state of the box collider
            _boundLocalMin = _spawnArea.center - _spawnArea.size * 0.5f;
            _boundLocalMax = _spawnArea.center + _spawnArea.size * 0.5f;
            return;
        }

        // deform spawn area box collider
        _spawnArea.transform.position = spawnAreaData._center;
        _spawnArea.size = spawnAreaData._size;
        _spawnArea.transform.rotation = spawnAreaData._rotation;
        
        // calculate min and max corner
        _boundLocalMin = spawnAreaData._center - spawnAreaData._size * 0.5f;
        _boundLocalMax = spawnAreaData._center + spawnAreaData._size * 0.5f;
    }
    
    /// <summary>
    /// create instance of a prefab in the argument
    /// </summary>
    /// <param name="spawnObject"></param>
    /// <returns></returns>
    public GameObject Spawn(GameObject spawnObject, Transform parent)
    {
        GameObject newObject = Instantiate(spawnObject);

        // set transform of newObject, so that it is placed in the spawn area
        // calculate spawn position in the local space of _spawnArea
        _positionTemp.x = Random.Range(_boundLocalMin.x, _boundLocalMax.x);
        _positionTemp.y = Random.Range(_boundLocalMin.y, _boundLocalMax.y);
        _positionTemp.z = Random.Range(_boundLocalMin.z, _boundLocalMax.z);
        
        // apply translate and rotation of _spawnArea
        _positionTemp = _spawnArea.transform.TransformPoint(_positionTemp);

        // assign position and rotation
        newObject.transform.position = _positionTemp;
        newObject.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newObject.transform.SetParent(parent);

        return newObject;
    }
}
