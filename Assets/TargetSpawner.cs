using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private List<PlayerSubSpawnForTargets> spawnPoints;
    [SerializeField] private List<TargetBehaviour> targetPrefabs;
    [Header("example: prefab,row,delayTillNext")]
    [SerializeField] private List<string> spawnMechanics;
    [SerializeField] private UnityEngine.Events.UnityEvent onGameOver;

    public void SpawnRandomly() => StartCoroutine(SpawnRandomlyRoutine());

    public IEnumerator SpawnRandomlyRoutine()
    {
        while (spawnMechanics.Count > 0)
        {
            float delay = HandlePart(spawnMechanics[0]);
            spawnMechanics.RemoveAt(0);
            yield return new WaitForSeconds(delay);
        }
        onGameOver?.Invoke();
        yield return null;
    }

    private float HandlePart(string partString)
    {
        var parts = partString.Split(',');
        int prefabIndex = Int32.Parse(parts[0]);
        int rowIndex = Int32.Parse(parts[1]);
        SpawnTarget(rowIndex - 1, prefabIndex - 1);
        return float.Parse(parts[2]);
    }

    private void SpawnTarget(int row, int prefabNum)
    {
        if (row >= spawnPoints.Count) return;
        var item = spawnPoints[row];
        var prefab = targetPrefabs[prefabNum];
        item.Spawn(prefab);
    }
}
