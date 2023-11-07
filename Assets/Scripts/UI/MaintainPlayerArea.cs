using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaintainPlayerArea : MonoBehaviour
{
    [SerializeField] private Vector2 screenBufferDist = Vector2.zero;
    [SerializeField] private Vector2 center = Vector2.zero;
    [SerializeField] private Vector2 maxBox = Vector2.zero;
    [SerializeField] private Vector2 minBox = Vector2.zero;
    [SerializeField] private float lerpSpeed = 0.5f;
    public void Update()
    {
        ClampScreen();
    }
    public void ClampScreen()
    {
        // Calculate average player position.
        PlayerInput[] players = GameObject.FindObjectsOfType<PlayerInput>();
        Vector2 playerPositionSum = Vector2.zero;
        foreach (var player in players) {
            Vector3 pos = player.transform.position;
            Vector2 xy = new Vector2(pos.x, pos.y);
            playerPositionSum += xy;
        }
        Vector2 avgPos = playerPositionSum / players.Length;

        Camera cam = Camera.main;

        // Find furthest player width, and furthest player height
        float maxHalfWidth = 0;
        float maxHalfHeight = 0;
        foreach (var player in players)
        {
            Vector3 pos = player.transform.position;
            Vector2 xy = new Vector2(pos.x, pos.y);
            //Vector2 ctp = xy - avgPos; //center to player
            Vector2 ctp = xy - Vector2.zero; //center to player
            maxHalfWidth = Mathf.Max(maxHalfWidth, Mathf.Abs(ctp.x) + screenBufferDist.x);
            maxHalfHeight = Mathf.Max(maxHalfHeight, Mathf.Abs(ctp.y) + screenBufferDist.y);
        }

        // Set camera scale
        float targetWidth = Mathf.Clamp(maxHalfWidth * 2, minBox.x, maxBox.x);
        float targetHeight = Mathf.Clamp(maxHalfHeight * 2, minBox.y, maxBox.y);
        float targetAspect = targetWidth / targetHeight;
        float finalOrthoSize = cam.orthographicSize;
        //Ensures camera sees entire target width and target height. 
        if (targetAspect >= cam.aspect)
            finalOrthoSize = targetWidth / (2 * cam.aspect);
        else
            finalOrthoSize = targetHeight / 2;

        //Ensures camera fits inside maxWidth and maxHeight?
        float worldWidth = finalOrthoSize * 2 * cam.aspect;
        float worldHeight = finalOrthoSize * 2;
        if (worldHeight > maxBox.y + 0.1)
        {
            // clamp just height down
            finalOrthoSize = maxBox.y / 2;
        } else if (worldWidth > maxBox.x + 0.1)
        {
            finalOrthoSize = maxBox.x / (2 * cam.aspect);
            // clamp just width down
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, finalOrthoSize, lerpSpeed);
    }
}
