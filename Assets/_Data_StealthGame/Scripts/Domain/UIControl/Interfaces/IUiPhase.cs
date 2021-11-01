using System;
/// <summary>
/// control single ui phase
/// </summary>
public interface IUiPhase
{
    // notify the action to move forward phase
    event EventHandler _onMoveForward;

    // notify the action to move backward phase
    event EventHandler _onMoveBackward;

    /// <summary>
    /// display this ui phase
    /// </summary>
    void Display();

    /// <summary>
    /// hide this ui phase
    /// </summary>
    void Hide();

    /// <summary>
    /// execute process to go to the next phase from a button
    /// </summary>
    void MoveForward();

    /// <summary>
    /// execute process o go back to the previous phase from a button
    /// </summary>
    void MoveBackward();
}
