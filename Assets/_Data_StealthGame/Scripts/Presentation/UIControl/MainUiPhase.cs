using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control main menu ui phase
/// </summary>
public class MainUiPhase : MonoBehaviour, IUiPhase
{

    // notify the action to move forward phase
    public event EventHandler _onMoveForward;

    // notify the action to move backward phase
    public event EventHandler _onMoveBackward;

    [SerializeField, Tooltip("identify role of this phase")]
    private MainUiPanelPhase _phaseType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public void Display()
    {

    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public void Hide()
    {

    }

    /// <summary>
    /// execute process to go to the next phase from a button
    /// </summary>
    public void MoveForward()
    {
        EventArgs args = EventArgs.Empty;
        _onMoveForward?.Invoke(this, args);
    }

    /// <summary>
    /// execute process o go back to the previous phase from a button
    /// </summary>
    public void MoveBackward()
    {
        EventArgs args = EventArgs.Empty;
        _onMoveBackward?.Invoke(this, args);
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
