using System;
using System.Timers;
/// <summary>
/// count time of gameplay, and notify if it reaches at the time limit
/// </summary>
public class TimeLimitCounter : IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // current play time of this play
    private float _currentPlayTime;

    // time limit of the gameplay
    private float _timeLimit;

    // whether time counting is paused
    private bool _isPaused;

    // play time counting timer
    private Timer _timeCountTimer;

    private int incrementTimeMilliSec = 100;

    /// <summary>
    /// constructor
    /// </summary>
    public TimeLimitCounter()
    {
        
    }

    /// <summary>
    /// destructor
    /// </summary>
    ~TimeLimitCounter()
    {

    }

    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }

    /// <summary>
    /// set time limit
    /// </summary>
    /// <param name="timeLimit"></param>
    public void SetTimeLimit(float timeLimit)
    {
        _timeLimit = timeLimit;
    }

    /// <summary>
    /// start counting playtime
    /// </summary>
    public void StartCount()
    {
        _currentPlayTime = 0f;

        _timeCountTimer = new Timer(incrementTimeMilliSec);
        _timeCountTimer.Elapsed += Count;
        _timeCountTimer.Disposed += NotifyGamePlayEnd;
        _timeCountTimer.Start();
    }

    /// <summary>
    /// temporarily resume counting playtime
    /// </summary>
    public void PauseCount()
    {
        _isPaused = true;
        _timeCountTimer.Stop();
    }

    /// <summary>
    /// resume counting playtime
    /// </summary>
    public void ResumeCount()
    {
        _isPaused = false;
        _timeCountTimer.Start();
    }

    /// <summary>
    /// terminate counting playtime
    /// </summary>
    public void QuitTimeCount()
    {
        _isPaused = false;
        _timeCountTimer.Stop();
        _timeCountTimer.Disposed -= NotifyGamePlayEnd;
        _timeCountTimer.Dispose();
    }

    private void Count(object sender, EventArgs args)
    {
        _currentPlayTime += (float)incrementTimeMilliSec * 0.001f;
        DebugLog.Info(this.ToString(), "_currentPlayTime: " + _currentPlayTime);

        if(_currentPlayTime >= _timeLimit)
        {
            _timeCountTimer.Stop();
            _timeCountTimer.Elapsed -= Count;
            _timeCountTimer.Dispose();
        }
    }

    /// <summary>
    /// notify the end of the gameplay
    /// </summary>
    private void NotifyGamePlayEnd(object sender, EventArgs args)
    {
        GamePlayStateEventArgs gamePlayStateargs = new GamePlayStateEventArgs(GamePlayState.AfterPlay);
        _onGamePlayStateChange?.Invoke(this, gamePlayStateargs);
        _timeCountTimer.Disposed -= NotifyGamePlayEnd;
    }
}
