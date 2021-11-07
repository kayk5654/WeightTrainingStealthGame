using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// provide a feature to spawn scene object
/// </summary>
public class ObjectSpawnHandler : MonoBehaviour
{
    
    
    /// <summary>
    /// create instance of a prefab in the argument
    /// </summary>
    /// <param name="spawnObject"></param>
    /// <returns></returns>
    public GameObject Spawn(GameObject spawnObject)
    {
        GameObject newObject = Instantiate(spawnObject);

        // set transform of newObject, so that it is placed in the spawn area

        return newObject;
    }
}
