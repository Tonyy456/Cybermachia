using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;

public class TIL_FSMController : MonoBehaviour
{
    [SerializeField] private FSMBehaviour FSMScript;
    [SerializeField] private StateSO countdownState;
    [SerializeField] private StateSO fightingState;
    [SerializeField] private StateSO summaryState;
    [SerializeField] private StateSO endState;

    public void Awake()
    {
    }
}
