using UnityEngine;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// </summary>
public class WorkoutNavigationUiLinker : ISceneObjectLinker<WorkoutNavigationUiController, WorkoutUiPhase>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(WorkoutNavigationUiController parentObject)
    {
        WorkoutUiPhase[] phases = Resources.FindObjectsOfTypeAll(typeof(WorkoutUiPhase)) as WorkoutUiPhase[];

        foreach (WorkoutUiPhase phase in phases)
        {
            // skip if the found phase isn't loaded in any scenes
            if (string.IsNullOrEmpty(phase.gameObject.scene.name)) { continue; }
            parentObject.SetUiPhase(phase);
        }
    }
}
