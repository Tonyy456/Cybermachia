using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tony;
using System;

[CustomEditor(typeof(ClampScreenBehavior))]
public class CameraResizeDebug : ButtonScriptHandler<ClampScreenBehavior>
{
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Resize", script.ClampScreen);
        } 
    }
}

[CustomEditor(typeof(PixelPerfectCameraClamper))]
public class PixelPerfectCameraResizeEditorDebug : ButtonScriptHandler<PixelPerfectCameraClamper>
{
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Resize", script.ClampScreen);
        }
    }
}

[CustomEditor(typeof(MaintainPlayerArea))]
public class CameraPlayerMaintainDebug : ButtonScriptHandler<MaintainPlayerArea>
{
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

[CustomEditor(typeof(TargetSpawner))]
public class TargetSpawnerDebugButton : ButtonScriptHandler<TargetSpawner>
{
    protected override void InitializeButtons()
    {
        if (script)
        {
            AddButton("Log Game Time", script.LogEstimatedGameTime);
        }
    }
}
