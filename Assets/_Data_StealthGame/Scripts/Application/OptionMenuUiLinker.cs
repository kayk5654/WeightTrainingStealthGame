using UnityEngine;

public class OptionMenuUiLinker : ISceneObjectLinker<OptionMenuUiController, HandOptionMenuUi>
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
