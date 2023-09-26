using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Machia.Input;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private MachiaInputActions input;

    // Start is called before the first frame update
    void Start()
    {
        input = new MachiaInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        // add input for player here
    }
}
