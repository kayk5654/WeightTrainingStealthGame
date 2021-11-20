using System;
/// <summary>
/// count time of current gameplay
/// </summary>
public interface IPlayTimeCounter
{
    // notify that the gameplay ends
    event EventHandler<GamePlayStateEventArgs> _onGamePlayEnd;

    /// <summary>
    /// set time limit of this play
    /// </summary>
    /// <param name="timeLimit"></param>
    void SetTimeLimit(float timeLimit);

    /// <summary>
    /// start counting playtime
    /// </summary>
    void StartCount();
    
    /// <summary>
    /// pause counting playtime
    /// </summary>
    void PauseCount();

    /// <summary>
    /// resume counting playtime
    /// </summary>
    void ResumeCount();

    /// <summary>
    /// terminate counting playtime
    /// </summary>
    void QuitCount();
}
