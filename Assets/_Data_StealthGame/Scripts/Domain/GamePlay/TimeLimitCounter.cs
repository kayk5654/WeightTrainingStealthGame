using System;
using System.Threading;
using System.Threading.Tasks;
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

    // cancellation token
    private CancellationTokenSource _cancellationTokenSource;

    // play time counting task
    private Task _timeCountTask;

    // whether time counting is paused
    private bool _isPaused;

    // do something in the main thread
    private SynchronizationContext _mainContext;


    /// <summary>
    /// constructor
    /// </summary>
    public TimeLimitCounter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _mainContext = SynchronizationContext.Current;
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
        //_timeCountTask = Task.Run(() => CountTime(_cancellationTokenSource.Token));
        _timeCountTask = new Task(() => CountTime(_cancellationTokenSource.Token));
        _timeCountTask.Start();
    }

    /// <summary>
    /// temporarily resume counting playtime
    /// </summary>
    public void PauseCount()
    {
        _isPaused = true;
    }

    /// <summary>
    /// resume counting playtime
    /// </summary>
    public void ResumeCount()
    {
        _isPaused = false;
    }

    /// <summary>
    /// terminate counting playtime
    /// </summary>
    public void QuitTimeCount()
    {
        _isPaused = false;
        _cancellationTokenSource.Cancel();
        //_timeCountTask.Dispose();
    }

    /// <summary>
    /// actual process to count playtime
    /// </summary>
    private async Task CountTime(CancellationToken token)
    {
        // count playtime
        int incrementTimeMilliSec = 100;
        while(_currentPlayTime < _timeLimit)
        {
            if (token.IsCancellationRequested) { break; }

            await Task.Delay(incrementTimeMilliSec);

            if (!_isPaused)
            {
                _currentPlayTime += (float)incrementTimeMilliSec * 0.001f;
                DebugLog.Info(this.ToString(), "_currentPlayTime: " + _currentPlayTime);
            }
            
            if (token.IsCancellationRequested) { break; }
        }

        // notify the end of the gameplay
        _mainContext.Post(_ => NotifyGamePlayEnd(), null);
    }

    /// <summary>
    /// notify the end of the gameplay
    /// </summary>
    private void NotifyGamePlayEnd()
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.AfterPlay);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
