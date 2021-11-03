using UnityEngine;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// </summary>
public class MainUiLinker : ISceneObjectLinker<MainUiController>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(MainUiController parentObject)
    {
        MainUiPhase[] phases = Resources.FindObjectsOfTypeAll(typeof(MainUiPhase)) as MainUiPhase[];

        foreach (MainUiPhase phase in phases)
        {
            // skip if the found phase isn't loaded in any scenes
            if(string.IsNullOrEmpty(phase.gameObject.scene.name)) { continue; }
            parentObject.SetUiPhase(phase);

            // set reference of ExerciseInfoSetter
            ExerciseInfoSetter exerciseInfoSetter = phase.GetComponent<ExerciseInfoSetter>();
            if (exerciseInfoSetter)
            {
                parentObject.SetExerciseInfoSetter(exerciseInfoSetter);
            }
        }


    }
}
