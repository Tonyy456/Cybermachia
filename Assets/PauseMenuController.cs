using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private InputAction thing;
    [SerializeField] private UnityEvent onPaused;
    [SerializeField] private UnityEvent onUnPaused;


    public void Start()
    {
        thing.performed += onthing;
        thing.Enable();
        HandleIsActive();
    }

    public void OnDestroy()
    {
        thing.performed -= onthing;
        thing.Disable();
    }
    public void onthing(InputAction.CallbackContext e)
    {
        menu.SetActive(!menu.activeInHierarchy);
        HandleIsActive();
    }

    public void HandleIsActive()
    {
        if (menu.activeInHierarchy)
        {
            onPaused?.Invoke();
        }
        else
        {
            onUnPaused?.Invoke();
        }
    }
}
