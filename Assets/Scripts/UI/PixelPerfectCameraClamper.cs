using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectCameraClamper : MonoBehaviour
{
    [SerializeField] private float maxAllowedWidth;
    [SerializeField] private float maxAllowedHeight;
    [SerializeField] private float tilesWide;
    [SerializeField] private float tilesTall;
    [SerializeField] private bool favorBiggerArea;
    [SerializeField] private PixelPerfectCamera pixelCam;

    private int counter = 0;
    public void ClampScreen()
    {
        counter = 0;
        var cam = Camera.main;
        var final = getFinalTileDim(cam);
        var f_tilesWide = final.width;
        var f_tilesTall = final.height;
        pixelCam.refResolutionX = (int)f_tilesWide * pixelCam.assetsPPU;
        pixelCam.refResolutionY = (int)f_tilesTall * pixelCam.assetsPPU;
        debug($"final values: {f_tilesWide},{f_tilesTall}");
        
        float unitHeight = cam.orthographicSize * 2;
        float unitWidth = cam.orthographicSize * 2 * cam.aspect;
        debug($"[target===actual] tiles wide: {tilesWide}==={unitWidth}");
        debug($"[target===actual] tiles tall: {tilesTall}==={unitHeight}");
    }
    private (float width, float height) getFinalTileDim(Camera cam)
    {
#if UNITY_EDITOR
        /*
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        */
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
#else
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
#endif

        Debug.Log("Screen Width: " + screenWidth);
        Debug.Log("Screen Height: " + screenHeight);
        float screenAspect = (float)screenWidth/(float)screenHeight;
        float userRequestedAspect = tilesWide / tilesTall;
        float w = tilesWide;
        float h = tilesTall;
        if (userRequestedAspect > screenAspect)
        { //Width is too large.  Shrink width or increase height.
            if(favorBiggerArea)
            { // increase height
                h = w / screenAspect;
            } 
            else
            { // decrease width
                w = screenAspect * h;
            }
        }
        else if (userRequestedAspect < screenAspect)
        { // Height too large. Shrink height or increase width
            if (favorBiggerArea)
            { // increase width
                w = screenAspect * h;
            }
            else
            { // decrease height
                h = w / screenAspect;
            }
        }

        // shrink to fit into max width and height allowance.
        if (w > maxAllowedWidth)
        {
            w = maxAllowedWidth;
            h = w / screenAspect;
        }
        if ( h > maxAllowedHeight)
        {
            h = maxAllowedHeight;
            w = screenAspect * h;
        }
        return (w, h);
    }
    private void debug(object log)
    {
        Debug.Log($"[{counter++}] {log}\n");
    }
}
