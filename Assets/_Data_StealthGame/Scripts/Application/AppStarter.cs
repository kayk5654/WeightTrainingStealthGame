using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// initialize settings and classes on starting this app
/// </summary>
public class AppStarter : MonoBehaviour
{
    /// <summary>
    /// initialization on MonoBehaviour
    /// </summary>
    private void Start()
    {
        StartApp();
    }

    /// <summary>
    /// initialize app logic
    /// </summary>
    private void StartApp()
    {
        AppManager appManager = new AppManager(this);
    }

    /// <summary>
    /// quit this app
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
}
