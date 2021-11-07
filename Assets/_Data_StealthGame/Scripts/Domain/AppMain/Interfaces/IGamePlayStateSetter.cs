using System;
/// <summary>
/// enable to change the gameplay state and notify it
/// </summary>
public interface IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    void SetGamePlayState(GamePlayState gamePlayState);
}
