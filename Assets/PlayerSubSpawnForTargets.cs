using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubSpawnForTargets : MonoBehaviour
{
    [SerializeField] private bool reverse = false;
    public void Spawn(TargetBehaviour behaviour, float speedScale = 1)
    {
        TargetBehaviour script = GameObject.Instantiate(behaviour);
        script.transform.position = this.transform.position;
        script.Init(reverse, speedScale);
    }
}
