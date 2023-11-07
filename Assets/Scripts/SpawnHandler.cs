using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnPointParent;

    public Vector2 GetSpawnLocation()
    {
        Vector2 location = Vector2.zero;
        int index = Random.Range(0, spawnPointParent.childCount);
        Vector3 pos = spawnPointParent.GetChild(index).transform.position;
        location = new Vector2(pos.x, pos.y);
        return location;
    }
}
