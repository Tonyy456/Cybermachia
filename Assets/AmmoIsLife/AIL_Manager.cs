using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIL_Manager : MonoBehaviour
{
    public void EnablePlayers(bool enabled = true)
    {
        var scripts = GameObject.FindObjectsOfType<AIL_PlayerController>();
        foreach (var script in scripts)
        {
            Debug.Log(script.name);
            script.EnableAllInput(enabled);
            script.EnableHealth(enabled);
        }
    }
}
