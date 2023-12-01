using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeGameController : MonoBehaviour
{
    public void PauseGame()
    {
        ChomBombAgent[] agents = GameObject.FindObjectsOfType<ChomBombAgent>();
        foreach (var agent in agents) agent.Pause = true;
        Time.timeScale = 0;
    }
    public void UnPuaseGame()
    {
        ChomBombAgent[] agents = GameObject.FindObjectsOfType<ChomBombAgent>();
        foreach (var agent in agents) agent.Pause = false;
        Time.timeScale = 1;
    }
}
