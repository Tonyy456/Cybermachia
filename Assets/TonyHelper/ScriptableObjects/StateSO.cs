using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tony
{
    [CreateAssetMenu(fileName = "State", menuName = "FSM/State")]
    public class StateSO : ScriptableObject
    {
        public string Name;
        public List<StateTransition> Transitions;
        public Action OnEnter { get; set; }
        public Action OnExit { get; set; }
    }
}

