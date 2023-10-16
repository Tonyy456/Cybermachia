using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CyberMachia;
using UnityEngine.Events;

public class ReadyManager : MonoBehaviour
{
    private static ReadyManager instance;
    public static ReadyManager Instance
    {
        get
        {
            return instance;
        }
    }
    public void Awake()
    {
        if (instance == null) instance = this;
    }


    [Header("Settings")]
    [Range(1, 4)]
    [SerializeField] private int expectedPlayerCount = 2;

    [Header("DEBUG, DONT EDIT")]
    [SerializeField] private List<SlotController> controllers = new List<SlotController>();
    [SerializeField] private List<bool> statuses = new List<bool>();

    [Header("Events")]
    public UnityEvent onAllReady;

    public void AddSlot(SlotController controller)
    {
        if (!controllers.Contains(controller))
        {
            statuses.Add(false);
            controllers.Add(controller);
        }
    }

    public void SetReadyStatus(SlotController controller, bool status)
    {
        int index = controllers.FindIndex(x => x == controller);
        if (index >= 0)
        {
            statuses[index] = status;
        }

        int numPlayers = statuses.Count;
        if (numPlayers < expectedPlayerCount) return;
        int numReady = statuses.FindAll(x => x == true).Count;
        if (numReady == numPlayers) onAllReady.Invoke();

    }

    public void RemoveSlot(SlotController controller)
    {
        int index = controllers.FindIndex(x => x == controller);
        if (index >= 0)
        {
            controllers.RemoveAt(index);
            statuses.RemoveAt(index);
        }
    }

}

