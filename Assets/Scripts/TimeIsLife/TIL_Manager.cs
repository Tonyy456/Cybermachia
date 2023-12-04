using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIL_Manager : MonoBehaviour
{
    public void EnablePlayers(bool enabled = true)
    {
        var scripts = GameObject.FindObjectsOfType<TIL_PlayerController>();
        foreach(var script in scripts)
        {
            script.EnableAllInput(enabled);
            script.EnableHealthTick();
        }
    }
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            EnablePlayers(false);
        }
        else
        {
            Time.timeScale = 1;
            EnablePlayers(true);
        }
    }
}
