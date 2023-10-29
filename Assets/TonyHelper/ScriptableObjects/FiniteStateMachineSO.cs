using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace Tony
{
    [CreateAssetMenu(fileName = "FSM", menuName = "FSM/FiniteStateMachine")]
    public class FiniteStateMachineSO : ScriptableObject
    {
        public string Name;
        public StateSO StartState;
        public List<StateTransition> AnyStateTransitions;
    }
}
