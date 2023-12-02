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
    public void Play(string name)
    {
        TonySoundClip match = sounds.Find(x => x.name == name);
        Debug.Log(name);
        Debug.Log("match: " + match);
        if(match != null)
        {
            source.PlayOneShot(match.clip);
        }
    }
}
