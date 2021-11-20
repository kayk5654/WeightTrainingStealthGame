using System;
/// <summary>
///  count time until it reaches at the threshold, then notify
/// </summary>
public interface ITimeCountChecker
{
    // callback when the target object is looked at longer than the threshold
    event EventHandler _onTimeCountEnd;


    /// <summary>
    /// start counting time
    /// </summary>
    void StartTimeCount();

    /// <summary>
    /// stop counting time
    /// </summary>
    void StopTimeCount();

    /// <summary>
    /// reset counting time
    /// </summary>
    void ResetTimeCount();

    /// <summary>
    /// get time phase in the range of 0 to 1
    /// </summary>
    /// <returns></returns>
    float GetNormalizedTime();

}
