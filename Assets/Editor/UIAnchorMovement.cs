using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/* Author: Anthony D'Alesandro
 * 
 * Unity helper method that moves anchor points of UI elements.
 * 
 * shift + q = move corners and anchors to parent's corners.
 * shift + / = move anchors to corners of image.
 */
namespace Machia.UI
{
    public class UIAnchorMovement : MonoBehaviour
    {
        [MenuItem("uGUI/Stretch To Parent #q")]
        static void StretchToParent()
        {
            foreach (Transform transform in Selection.transforms)
            {
                RectTransform t = transform as RectTransform;

                if (t == null) return;

                t.anchorMin = new Vector2(0, 0);
                t.anchorMax = new Vector2(1, 1);
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }

        [MenuItem("uGUI/Stretch To AnchorsP #/")]
        static void StretchAnchors()
        {
            foreach (Transform transform in Selection.transforms)
            {
                RectTransform t = transform as RectTransform;
                RectTransform tp = transform.parent.GetComponent<RectTransform>();
                if (t == null || tp == null) return;

                Debug.Log($"Old values:\n anchorMin:{t.anchorMin} anchorMax: {t.anchorMax}");
                t.anchorMin += t.offsetMin / tp.rect.size;
                t.anchorMax += (t.offsetMax / tp.rect.size);
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }
    }
}