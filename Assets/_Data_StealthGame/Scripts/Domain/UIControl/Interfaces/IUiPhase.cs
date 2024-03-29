﻿using System;
/// <summary>
/// control single ui phase
/// </summary>
public interface IUiPhase
{
    // notify the action to move to the selected phase
    event EventHandler<UiPhaseEventArgs> _onMoveToSelectedPhase;

    /// <summary>
    /// display this ui phase
    /// </summary>
    void Display();

    /// <summary>
    /// hide this ui phase
    /// </summary>
    void Hide();

    /// <summary>
    /// execute process to go to the selected phase from a button 
    /// </summary>
    /// <param name="phaseId"></param>
    void MoveToSelectedPhase(int phaseId);

    /// <summary>
    /// get phase id among the same ui phase group managed by IMultiPhaseUi
    /// </summary>
    /// <returns></returns>
    int GetPhaseId();
}
