using UnityEngine;
using System.Linq;
using System.Collections.Generic;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// </summary>
public class WorkoutNavigationUiLinker : ISceneObjectLinker<WorkoutNavigationUiController>
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

        WorkoutStarter[] workoutStarters = Resources.FindObjectsOfTypeAll<WorkoutStarter>();
        workoutStarters = workoutStarters.Where(starter => !string.IsNullOrEmpty(starter.gameObject.scene.name)).ToArray();

        GamePlayEndHandler[] gamePlayEndHandlers = Resources.FindObjectsOfTypeAll<GamePlayEndHandler>();
        gamePlayEndHandlers = gamePlayEndHandlers.Where(handler => !string.IsNullOrEmpty(handler.gameObject.scene.name)).ToArray();

        List<IGamePlayStateSetter> setters = new List<IGamePlayStateSetter>();
        setters.AddRange(workoutStarters);
        setters.AddRange(gamePlayEndHandlers);
        parentObject.SetGamePlayStateSetter(setters.ToArray());
    }
}
