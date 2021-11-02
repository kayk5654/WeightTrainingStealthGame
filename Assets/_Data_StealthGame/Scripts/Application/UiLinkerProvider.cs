using UnityEngine;
/// <summary>
/// link a ui control class in the domain layer with relative ui objects in the scene
/// depending on the given class, different process is executed.
/// </summary>
public class UiLinkerProvider
{
    // link main ui classes
    private MainUiLinker _mainUiLinker;

    // link workout navigation ui classes
    private WorkoutNavigationUiLinker _workoutNavigationUiLinker;

    private OptionMenuUiLinker _optionMenuUiLinker;

    /// <summary>
    /// constructor
    /// </summary>
    public UiLinkerProvider()
    {
        _mainUiLinker = new MainUiLinker();
        _workoutNavigationUiLinker = new WorkoutNavigationUiLinker();
        _optionMenuUiLinker = new OptionMenuUiLinker();
    }

    /// <summary>
    /// link main ui classes
    /// </summary>
    /// <param name="mainUiController"></param>
    public void LinkObject(MainUiController mainUiController)
    {
        _mainUiLinker.LinkObject(mainUiController);
    }

    /// <summary>
    /// link workout navigation ui classes
    /// </summary>
    /// <param name="workoutNavigationUiController"></param>
    public void LinkObject(WorkoutNavigationUiController workoutNavigationUiController)
    {
        _workoutNavigationUiLinker.LinkObject(workoutNavigationUiController);
    }

    public void LinkObject(OptionMenuUiController optionMenuUiController)
    {
        _optionMenuUiLinker.LinkObject(optionMenuUiController);
    }
}
