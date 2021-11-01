using System.Collections;
using System.Collections.Generic;
/// <summary>
/// control ui with multiple phases
/// </summary>
public interface IMultiPhaseUi
{
    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    void DisplayUiPhase(int phaseIndex);
}
