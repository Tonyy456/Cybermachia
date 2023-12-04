using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TonySoundClip
{
    public string name;
    public AudioClip clip;
}
public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private List<TonySoundClip> sounds;
    [SerializeField] private AudioSource source;
    private static SoundEffectManager instance;
    public static bool TryPlay(string name)
    {
        if (instance == null)
        {
            Debug.Log("Cant play audio. No sound manager");
            return false;
        }
        else
        {
            bool worked = instance.Play(name);
            if (!worked) Debug.Log("Cant play audio. Wasnt found");
            return worked;
        }
    }
    public static SoundEffectManager Instance
    {
        get
        {
            return instance;
        }
    }
    public void Awake()
    {
        instance = this;
    }
    public bool Play(string name)
    {
        TonySoundClip match = sounds.Find(x => x.name == name);

        if(match != null)
        {
            source.PlayOneShot(match.clip);
            return true;
        }
        return false;
    }
}
