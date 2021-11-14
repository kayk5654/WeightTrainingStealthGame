using System;
/// <summary>
/// notify input for in-game actions to upper classes
/// </summary>
public interface IInGameInputBase
{
    // notify that the pushing action is detected
    event EventHandler _onPush;

    // notify that the start holding action is detected
    event EventHandler _onStartHold;

    // notify that the stop holding action is detected
    event EventHandler _onStopHold;
}
