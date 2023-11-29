using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMenuController : MonoBehaviour
{
    [SerializeField] private GameObject turnOn;
    [SerializeField] private TMPro.TMP_Text text;
    public void Error(string message)
    {
        turnOn.SetActive(true);
        text.text = message;
    }
}
