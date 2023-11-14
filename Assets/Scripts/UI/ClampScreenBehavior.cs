using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampScreenBehavior : MonoBehaviour
{
    [SerializeField] private float targetWidth;
    [SerializeField] private float targetHeight;
    [SerializeField] private bool clampOnStart;
    public void Start()
    {
        if (clampOnStart) ClampScreen();
    }
    public void ClampScreen()
    {
        Camera cam = Camera.main;
        var aspect = cam.aspect;
        Debug.Log(aspect);
        Debug.Log(cam.aspect);
        float worldWidth = cam.orthographicSize * 2 * aspect;
        float worldHeight = cam.orthographicSize * 2;
        Debug.Log(worldWidth);
        Debug.Log(worldHeight);

        float targetAspect = targetWidth / targetHeight;
        if (targetAspect >= cam.aspect)
        { // Need to clamp width down
            cam.orthographicSize = targetWidth / (2 * aspect);
        }
        else
        { // Need to clamp height down
            cam.orthographicSize = targetHeight / 2;
        }
    }
}
