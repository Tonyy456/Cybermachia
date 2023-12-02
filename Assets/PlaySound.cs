using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private string soundName;
    public void Play()
    {
        SoundEffectManager.Instance?.Play(soundName);
    }
}
