using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhasePart
{
    public int prefab;
    public int spawn;
    public float startDelay;
}

[Serializable]
public class Phase
{
    public List<PhasePart> parts;
}

[Serializable]
public class TargetPhaseEvent
{
    public int phase;
    public float startDelay;
}

public class TargetSpawner : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private List<PlayerSubSpawnForTargets> spawnPoints;
    [SerializeField] private List<TargetBehaviour> targetPrefabs;

    [Space]
    [Header("Phases and Game Control")]
    [SerializeField] private List<Phase> phases;
    [SerializeField] private List<TargetPhaseEvent> phaseEvents;
    [SerializeField] private float gameTimePadding;

    [Space]
    [Header("Events")]
    [SerializeField] private UnityEngine.Events.UnityEvent onGameOver;

    public void StartGame() => StartCoroutine(SpawnPhases());

    public IEnumerator SpawnPhases()
    {
        for(int i = 0; i < phaseEvents.Count - 1; i++)
        {
            var phaseEvent = phaseEvents[i];
            yield return new WaitForSeconds(phaseEvent.startDelay);
            StartCoroutine(SpawnPhase(phaseEvent.phase - 1));
        }
        if (phaseEvents.Count > 0)
        {
            var finalPhase = phaseEvents[phaseEvents.Count - 1];
            yield return new WaitForSeconds(finalPhase.startDelay);
            StartCoroutine(SpawnPhase(finalPhase.phase - 1, true));
        }
        yield return null;
    }

    public IEnumerator SpawnPhase(int index, bool lastPhase = false)
    {
        if (index < 0 || index >= phases.Count) yield return null;
        Phase phase = phases[index];

        foreach(var part in phase.parts)
        {
            yield return new WaitForSeconds(part.startDelay);
            SpawnTarget(part.spawn - 1, part.prefab - 1);
        }
        if (lastPhase)
        {
            yield return new WaitForSeconds(gameTimePadding);
            onGameOver?.Invoke();
        }
        yield return null;
    }

    private void SpawnTarget(int row, int prefabNum)
    {
        if (row >= spawnPoints.Count) return;
        var item = spawnPoints[row];
        var prefab = targetPrefabs[prefabNum];
        item.Spawn(prefab);
    }
    //private float HandlePart(string partString)
    //{
    //    var parts = partString.Split(',');
    //    int prefabIndex = Int32.Parse(parts[0]);
    //    int rowIndex = Int32.Parse(parts[1]);
    //    SpawnTarget(rowIndex - 1, prefabIndex - 1);
    //    return float.Parse(parts[2]);
    //}


}
