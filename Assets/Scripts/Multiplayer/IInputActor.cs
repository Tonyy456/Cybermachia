using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Input
{
    public interface IInputActor 
    {
        public void Initialize(PlayerInput inputManager);
    }
}
