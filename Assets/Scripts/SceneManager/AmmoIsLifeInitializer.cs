using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;
using UnityEngine.InputSystem;

public class AmmoIsLifeInitializer : MonoBehaviour
{
    [SerializeField] private FSMBehaviour FSM;
    [SerializeField] private ClampScreenBehavior CameraClamper;

    public void Start()
    {
        // Fix Camera to fix screen better.
        CameraClamper.ClampScreen();
    }

    public void InitializeScene()
    {
        // Fix Camera to fix screen better.
        CameraClamper.ClampScreen();

        // Figure out map... generate it?

        FSM.Fire("initialized"); // transition to next state... if not already done.
    }
}
