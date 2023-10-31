using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampScreenBehavior : MonoBehaviour
{
    [SerializeField] private float targetWidth;
    [SerializeField] private float targetHeight;
    public void ClampScreen()
    {
        Camera cam = Camera.main;
        float worldWidth = cam.orthographicSize * 2 * cam.aspect;
        float worldHeight = cam.orthographicSize * 2;

        float targetAspect = targetWidth / targetHeight;
        if (targetAspect >= cam.aspect)
        { // Need to clamp width down
            cam.orthographicSize = targetWidth / (2 * cam.aspect);
        }
        else
        { // Need to clamp height down
            cam.orthographicSize = targetHeight / 2;
        }
    }
}
