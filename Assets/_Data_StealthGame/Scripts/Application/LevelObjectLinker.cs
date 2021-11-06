﻿using UnityEngine;
/// <summary>
/// set scene object reference for LevelManager
/// </summary>
public class LevelObjectLinker : ISceneObjectLinker<LevelManager>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(LevelManager parentObject)
    {
        NodesManager nodesManager = MonoBehaviour.FindObjectOfType<NodesManager>();
        
        // link player object manager
        parentObject.SetPlayerObjectManager(nodesManager);
        
        // link enemy manager
    }
}
