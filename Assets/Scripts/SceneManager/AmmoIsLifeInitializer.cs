using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;

public class AmmoIsLifeInitializer : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineSO FSM;

    public void InitializeScene()
    {
        // Figure out map

        // Spawn players

        FSM.Fire("next"); // transition to next state... if not already done.
    }
}
