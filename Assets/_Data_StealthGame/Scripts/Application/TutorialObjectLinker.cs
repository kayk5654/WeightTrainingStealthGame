using System.Linq;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// </summary>
public class TutorialObjectLinker : ISceneObjectLinker<TutorialUiController>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(TutorialUiController parentObject)
    {
        TutorialUiPhase[] phases = Resources.FindObjectsOfTypeAll(typeof(TutorialUiPhase)) as TutorialUiPhase[];

        foreach (TutorialUiPhase phase in phases)
        {
            // skip if the found phase isn't loaded in any scenes
            if (string.IsNullOrEmpty(phase.gameObject.scene.name)) { continue; }
            parentObject.SetUiPhase(phase);
        }
    }
}
