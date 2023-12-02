using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHolderHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> correspondingPlayerObjects;
    private int numPlayers = 0;
    public int NumPlayers {
        get
        {
            return numPlayers;
        }
        set
        {
            numPlayers = value;
            for(int i = 0; i < correspondingPlayerObjects.Count; i++)
            {
                var o = correspondingPlayerObjects[i];
                o.SetActive(i < value);
            }
        }
    }
    public void Start()
    {
        numPlayers = 0;
    }
    public void Refresh()
    {
        var things = GameObject.FindObjectsOfType<PlayerInput>();
        NumPlayers = things.Length;
        Debug.Log(NumPlayers);
    }

    public void Add(int dif)
    {
        NumPlayers += dif;
    }
}
