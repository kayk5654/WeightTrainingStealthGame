using System.Collections;
using System.Collections.Generic;
/// <summary>
/// control cursor
/// </summary>
public interface ICursor
{
    /// <summary>
    /// enable/disable cursor
    /// </summary>
    /// <param name="state"></param>
    void SetActive(bool state);
}
