using System;
/// <summary>
/// receive event to activate features for last several seconds of current gameplay
/// </summary>
public interface ILastRushEventReceiver
{
    /// <summary>
    /// callback of LastRushEvent
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void LastRushEventCallback(object sender, EventArgs args);
}
