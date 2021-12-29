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

    // link option menu ui classes
    private OptionMenuUiLinker _optionMenuUiLinker;

    // link tutorial phase classes
    private TutorialObjectLinker _tutorialObjectLinker;

    // link start game ui phase classes
    private StartGameUiLinker _startGameUiLinker;

    /// <summary>
    /// constructor
    /// </summary>
    public UiLinkerProvider()
    {
        _mainUiLinker = new MainUiLinker();
        _workoutNavigationUiLinker = new WorkoutNavigationUiLinker();
        _optionMenuUiLinker = new OptionMenuUiLinker();
        _tutorialObjectLinker = new TutorialObjectLinker();
        _startGameUiLinker = new StartGameUiLinker();
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

    /// <summary>
    /// link option menu ui classes
    /// </summary>
    /// <param name="optionMenuUiController"></param>
    public void LinkObject(OptionMenuUiController optionMenuUiController)
    {
        _optionMenuUiLinker.LinkObject(optionMenuUiController);
    }

    /// <summary>
    /// link tutorial phase classes
    /// </summary>
    /// <param name="tutorialUiController"></param>
    public void LinkObject(TutorialUiController tutorialUiController)
    {
        _tutorialObjectLinker.LinkObject(tutorialUiController);
    }

    /// <summary>
    /// link start game ui phase classes
    /// </summary>
    /// <param name="startGameUiController"></param>
    public void LinkObject(StartGameUiController startGameUiController)
    {
        _startGameUiLinker.LinkObject(startGameUiController);
    }
}
