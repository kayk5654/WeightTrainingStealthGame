using UnityEngine;
/// <summary>
/// set scene object reference for OptionMenuUiController
/// </summary>
public class OptionMenuUiLinker : ISceneObjectLinker<OptionMenuUiController>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(OptionMenuUiController parentObject)
    {
        HandOptionMenuUi optionMenu = MonoBehaviour.FindObjectOfType<HandOptionMenuUi>();
        parentObject.SetOptionMenuUi(optionMenu);
    }
}
