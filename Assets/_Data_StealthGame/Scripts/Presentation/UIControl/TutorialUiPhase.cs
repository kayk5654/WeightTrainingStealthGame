using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
/// <summary>
/// phase of tutorial
/// </summary>
public class TutorialUiPhase : MonoBehaviour, IUiPhase
{
    // notify the action to move to the selected phase
    public event EventHandler<UiPhaseEventArgs> _onMoveToSelectedPhase;

    [SerializeField, Tooltip("identify role of this phase")]
    protected TutorialPhase _phaseType;

    [SerializeField, Tooltip("root gameobject of this ui phase")]
    protected GameObject[] _rootObjects;

    [SerializeField, Tooltip("solver handler of the menu panel")]
    protected SolverHandler _solverHandler;

    [SerializeField, Tooltip("adjust ui panel transform")]
    private UITransformModifier _uiTransformModifier;

    [SerializeField, Tooltip("offset of solver handler")]
    private Vector3 _panelPositionOffset;

    [SerializeField, Tooltip("offset of solver handler")]
    private Vector3 _panelRotationOffset;

    protected virtual void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public virtual void Display()
    {
        foreach(GameObject root in _rootObjects)
        {
            root.SetActive(true);
        }

        if (_solverHandler)
        {
            _solverHandler.AdditionalOffset = _panelPositionOffset;
            _solverHandler.AdditionalRotation = _panelRotationOffset;
        }

        // if  UITransformModifier is used, only height can be adjusted currently
        if (_uiTransformModifier)
        {
            _uiTransformModifier._relativeHeight = _panelPositionOffset.y;
            _uiTransformModifier._localXAngle = _panelRotationOffset.x;
        }
    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public virtual void Hide()
    {
        foreach (GameObject root in _rootObjects)
        {
            root.SetActive(false);
        }
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
