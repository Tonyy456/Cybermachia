using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomVisionModule : VisionModule
{
    [SerializeField] private string targetTag;
    [SerializeField] private string boidTag;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggerEnter(collision, false);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        HandleTriggerEnter(collision, true);
    }

    private void HandleTriggerEnter(Collider2D collider, bool remove = false)
    {
        List<Transform> listToOperate;
        var hitTag = collider.transform.tag;
        if (hitTag == boidTag)
        {
            listToOperate = boids;
        }
        else if (hitTag == targetTag)
        {
            listToOperate = targets;
        }
        else
        {
            listToOperate = obstacles;
        }

        if (remove) listToOperate.Remove(collider.transform);
        else if (!listToOperate.Contains(collider.transform))
            listToOperate.Add(collider.transform);

    }
}
