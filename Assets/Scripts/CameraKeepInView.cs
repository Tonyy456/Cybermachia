using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeepInView : MonoBehaviour
{
    [SerializeField] private float item;
    public void AttemptResize()
    {
        Camera cam = Camera.main;
        int pixelW = cam.pixelWidth;

        Vector3 leftToRight = cam.ScreenToWorldPoint(Vector2.zero)
                          - cam.ScreenToWorldPoint(new Vector2(pixelW, 0));
        float width = leftToRight.magnitude;
    }
}
