using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// control main menu ui
/// </summary>
public class MainUiController : IMultiPhaseUi, IAppStateSetter, IExerciseInfoSetter
{
    // ui phases to control
    private Dictionary<MainUiPanelPhase, IUiPhase> _uiPhases;

    // event to notify the start of MainMenu phase
    public event EventHandler<AppStateEventArgs> _onAppStateChange;

    // notify information about the selected exercise
    public event EventHandler<ExerciseInfoEventArgs> _onExerciseSelected;

    // get information of the selected exercise
    private IExerciseInfoSetter _exerciseInfoSetter;


    /// <summary>
    /// constructor
    /// </summary>
    public MainUiController()
    {
        _uiPhases = new Dictionary<MainUiPanelPhase, IUiPhase>();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    ~MainUiController()
    {
        foreach(IUiPhase phase in _uiPhases.Values)
        {
            phase._onMoveToSelectedPhase -= MoveToSelectedPhase;
        }
        _exerciseInfoSetter._onExerciseSelected -= NotifySelectedExercise;
        _exerciseInfoSetter._onExerciseSelected -= NotifyGamePlayStart;
    }

    /// <summary>
    /// contain ui phase in the dictionary
    /// </summary>
    /// <param name="phase"></param>
    public void SetUiPhase(IUiPhase phase)
    {
        // check whether the phase id of the phase is valid
        if (phase.GetPhaseId() >= (int)MainUiPanelPhase.LENGTH || phase.GetPhaseId() < 0) { return; }

        _uiPhases.Add((MainUiPanelPhase)phase.GetPhaseId(), phase);

        // set callback of the button on the phase
        phase._onMoveToSelectedPhase += MoveToSelectedPhase;
    }

    /// <summary>
    /// set reference of _exerciseInfoSetter
    /// </summary>
    /// <param name="exerciseInfoSetter"></param>
    public void SetExerciseInfoSetter(IExerciseInfoSetter exerciseInfoSetter)
    {
        _exerciseInfoSetter = exerciseInfoSetter;
        // set callback
        _exerciseInfoSetter._onExerciseSelected += NotifySelectedExercise;
        _exerciseInfoSetter._onExerciseSelected += NotifyGamePlayStart;
    }

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void DisplayUiPhase(int phaseIndex)
    {
        // if the selected ui phase is none, hide all panels
        if(phaseIndex == (int)MainUiPanelPhase.None)
        {
            foreach (MainUiPanelPhase key in _uiPhases.Keys)
            {
                _uiPhases[key].Hide();
            }
            return;
        }
        
        // check whether the given phaseIndex is valid
        if (phaseIndex >= (int)MainUiPanelPhase.LENGTH || phaseIndex < 0) { return; }
        
        foreach(MainUiPanelPhase key in _uiPhases.Keys)
        {
            if ((int)key == phaseIndex)
            {
                _uiPhases[key].Display();
            }
            else
            {
                _uiPhases[key].Hide();
            }
        }
    }

    /// <summary>
    /// switch  ui phase to display depending on the button input from any ui phases under this class
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveToSelectedPhase(object sender, UiPhaseEventArgs args)
    {
        DisplayUiPhase(args._selectedPhaseId);
    }

    /// <summary>
    /// update the app state from the classes refer this
    /// </summary>
    /// <param name="appState"></param>
    public void SetAppState(AppState appState)
    {

    }

    /// <summary>
    /// notify selected exercise to the upper class
    /// </summary>
    private void NotifySelectedExercise(object sender, ExerciseInfoEventArgs args)
    {
        _onExerciseSelected?.Invoke(sender, args);
    }

    /// <summary>
    /// when the exercise selection is finalized, terminate MainMenu AppState and start gameplay
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NotifyGamePlayStart(object sender, ExerciseInfoEventArgs args)
    {
        AppStateEventArgs appStateArgs = new AppStateEventArgs(AppState.GamePlay);
        _onAppStateChange?.Invoke(this, appStateArgs);
    }
}
