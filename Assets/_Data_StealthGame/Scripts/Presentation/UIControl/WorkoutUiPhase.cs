using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control gameplay ui phase during workout
/// </summary>
public class WorkoutUiPhase : MonoBehaviour, IUiPhase
{
    // notify the action to move to the selected phase
    public event EventHandler<UiPhaseEventArgs> _onMoveToSelectedPhase;

    [SerializeField, Tooltip("identify role of this phase")]
    private WorkoutNavigationUiPanelPhase _phaseType;

    [SerializeField, Tooltip("root gameobject of this ui phase")]
    private GameObject _panel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public void Display()
    {
        _panel.SetActive(true);
    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public void Hide()
    {
        _panel.SetActive(false);
    }

    /// <summary>
    /// execute process to go to the selected phase from a button 
    /// </summary>
    /// <param name="phaseId"></param>
    public void MoveToSelectedPhase(int phaseId)
    {
        UiPhaseEventArgs args = new UiPhaseEventArgs(phaseId);
        _onMoveToSelectedPhase.Invoke(this, args);
    }

    /// <summary>
    /// get phase id among the same ui phase group managed by IMultiPhaseUi
    /// </summary>
    /// <returns></returns>
    public int GetPhaseId()
    {
        return (int)_phaseType;
    }
}
