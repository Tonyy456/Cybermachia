using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scene_ConnectPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager manager;

    public void Awake()
    {
        manager.onPlayerJoined += onPlayerJoined;
    }

    private void onPlayerJoined(PlayerInput obj)
    {
        
    }
}
