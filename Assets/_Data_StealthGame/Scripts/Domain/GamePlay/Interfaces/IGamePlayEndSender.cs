using System;
/// <summary>
/// notify the end of a gameplay
/// </summary>
public interface IGamePlayEndSender
{
    // notify the end of current gameplay to the upper classes
    event EventHandler<GamePlayEndArgs> _onGamePlayEnd;
}
