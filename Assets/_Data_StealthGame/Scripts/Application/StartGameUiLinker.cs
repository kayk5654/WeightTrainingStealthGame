using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameUiLinker : ISceneObjectLinker<StartGameUiController>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(StartGameUiController parentObject)
    {
        StartGameUiPhase[] phases = Resources.FindObjectsOfTypeAll(typeof(StartGameUiPhase)) as StartGameUiPhase[];

        foreach (StartGameUiPhase phase in phases)
        {
            // skip if the found phase isn't loaded in any scenes
            if (string.IsNullOrEmpty(phase.gameObject.scene.name)) { continue; }
            parentObject.SetUiPhase(phase);
        }
    }
}
