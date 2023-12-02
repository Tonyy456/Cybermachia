using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUpManager2 : MonoBehaviour
{
    [SerializeField] private GameObject textPopUpPrefab;

    public void HandlePopup(string text, Vector3 spawnAt, Color color)
    {
        GameObject o = GameObject.Instantiate(textPopUpPrefab);
        var script = o.GetComponent<TextPopUpBehaviour2>();
        Debug.Log(script);
        script.Initialize(text, spawnAt, color);
    }
    public void HandlePopup(string text, Vector3 spawnAt)
    {
        HandlePopup(text, spawnAt, Color.red);
    }
}
