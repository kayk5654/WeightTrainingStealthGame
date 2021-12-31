using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// </summary>
public class SettingsUiLinker : ISceneObjectLinker<SettingsUiController>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(SettingsUiController parentObject)
    {
        SettingsUiPhase[] phases = Resources.FindObjectsOfTypeAll(typeof(SettingsUiPhase)) as SettingsUiPhase[];

        foreach (SettingsUiPhase phase in phases)
        {
            // skip if the found phase isn't loaded in any scenes
            if (string.IsNullOrEmpty(phase.gameObject.scene.name)) { continue; }
            parentObject.SetUiPhase(phase);
        }
    }
}
