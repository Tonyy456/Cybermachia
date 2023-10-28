using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;

public class CameraResizeDebug : ButtonScriptHandler<CameraKeepInView>
{
    protected override void InitializeButtons()
    {
        AddButton("Resize", script.AttemptResize);
    }
}
