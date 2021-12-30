using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// control objects in the lock-on descriptions of the tutorial
/// </summary>
public class TutorialLockOnObjectControl : MonoBehaviour
{
    [SerializeField, Tooltip("gage cursor animator")]
    private Animator _gageCursor;

    // animator property to scale the cursor up
    private string _scaleUpProperty = "ScaleUp";

    // animator property to scale the cursor down
    private string _initProperty = "Init";


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// play focus animation of the gage cursor
    /// </summary>
    public void FocusGageCursor()
    {
        _gageCursor.SetTrigger(_scaleUpProperty);
    }

    /// <summary>
    /// play shrink animation of the gage cursor
    /// </summary>
    public void InitGageCursor()
    {
        _gageCursor.SetTrigger(_initProperty);
    }
}
