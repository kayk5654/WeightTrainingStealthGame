#if UNITY_EDITOR
using UnityEngine;
#endif
using System;
/// <summary>
/// display logs on the console
/// </summary>
public static class DebugLog
{
    /// <summary>
    /// display log
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="log"></param>
    public static void Info(string objectName, string log)
    {
#if UNITY_EDITOR
        Debug.Log(DateTime.Now + " / " + objectName + " / " + log);
#endif
    }

    /// <summary>
    /// display warning message
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="log"></param>
    public static void Warning(string objectName, string log)
    {
#if UNITY_EDITOR
        Debug.LogWarning(DateTime.Now + " / " + objectName + " / " + log);
#endif
    }

    /// <summary>
    /// display error log
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="log"></param>
    public static void Error(string objectName, string log)
    {
#if UNITY_EDITOR
        Debug.LogError(DateTime.Now + " / " + objectName + " / " + log);
#endif
    }
}
