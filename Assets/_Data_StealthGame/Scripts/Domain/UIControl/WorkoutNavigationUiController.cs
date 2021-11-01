using System.Collections;
using System.Collections.Generic;
/// <summary>
/// control ui for workout navigation during gameplay
/// </summary>
public class WorkoutNavigationUiController : IMultiPhaseUi
{
    // ui phases to control
    private IUiPhase[] _uiPhases;

    /// <summary>
    /// select ui phase to display
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void SetUiPhase(int phaseIndex)
    {
        for (int i = 0; i < _uiPhases.Length; i++)
        {
            if (i == phaseIndex)
            {
                _uiPhases[i].Display();
            }
            else
            {
                _uiPhases[i].Hide();
            }

        }
    }
}
