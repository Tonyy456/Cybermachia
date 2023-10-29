using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tony;
using System;

[CustomEditor(typeof(ClampScreenBehavior))]
public class CameraResizeDebug : ButtonScriptHandler<ClampScreenBehavior>
{
    //private override void InitializeButtons()
    //{
    //    AddButton("Resize", ((CameraKeepInView)target).AttemptResize);
    //}
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Resize", script.ClampScreen);
        } 
    }
}

[CustomEditor(typeof(PlayerState))]
public class PlayerStatePrintHealth : ButtonScriptHandler<PlayerState>
{
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Log Health", () =>
            {
                Debug.Log(script.Health);
            });
        }
    }
}

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerPauseUnPause : ButtonScriptHandler<PlayerController>
{
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Pause", script.PausePlayer);
            AddButton("Play", script.EnablePlayer);
        }
    }
}
