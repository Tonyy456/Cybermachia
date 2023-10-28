using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameDef", menuName = "Minigame")]
public class Minigame : ScriptableObject
{
    public string sceneName;
    public string minigameName;
    public Color displayColor = new Color(1f,1f,1f,1f);
}
