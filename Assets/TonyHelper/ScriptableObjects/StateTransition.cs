using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tony
{
    [Serializable]
    public class StateTransition
    {
        public string trigger;
        public StateSO state;
    }
}

