using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordePlayer : MonoBehaviour
{
    [SerializeField] private bool EnableInput = true;

    public void Start()
    {
        var comps = GetComponents<PlayerInputScript>();
        foreach(var comp in comps)
        {
            comp.EnableInput();
        }
    }
}
