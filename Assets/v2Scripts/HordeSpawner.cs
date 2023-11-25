using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class HordeSpawner : MonoBehaviour
{
    [Header("number=enemy type, '+' = round divider")]
    [SerializeField] private List<GameObject> enemyTypes;
    [SerializeField] private List<GameObject> dropItems;
    [Range(0f, 1f)]
    [SerializeField] private List<float> spawnChance;
    [SerializeField] private string controlString;
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    public UnityEvent OnRoundStart;
    public UnityEvent OnAllEnemiesSpawned;
    private int currentRound = 0;
    private List<string> rounds;

    public void OnValidate()
    {
        if (spawnChance.Count < dropItems.Count)
        {
            spawnChance.Add(0f);
        }
        if (spawnChance.Count > dropItems.Count)
        {
            spawnChance.RemoveAt(spawnChance.Count - 1);
        }
    }

    public int roundsLeft { 
        get
        {
            return rounds.Count - currentRound;
        } 
    }
    public void Start()
    {
        if (controlString.Length == 0) return;
        rounds = new List<string>(controlString.Split('+'));
        SpawnNextRound();
    }

    public IEnumerator HandleRoundSpawn()
    {
        OnRoundStart?.Invoke();
        string round = rounds[currentRound++];
        for (int i = 0; i < round.Length; i++)
        {
            spawnEnemy(round.Substring(i, 1));
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        }
        OnAllEnemiesSpawned?.Invoke(); 
        yield return null;
    }
    public void SpawnNextRound()
    {
        StartCoroutine(HandleRoundSpawn());
    }

    private void spawnEnemy(string type)
    {
        if (Int32.TryParse(type, out int result))
        {
            var enemyPrefab = enemyTypes[result - 1];
            var go = GameObject.Instantiate(enemyPrefab);
            var childIndex = UnityEngine.Random.Range(0, spawnPoints.childCount);
            var spawnPoint = spawnPoints.GetChild(childIndex);
            go.transform.position = spawnPoint.transform.position;
            for(int i = 0; i < dropItems.Count; i++)
            {
                var item = dropItems[i];
                var chance = spawnChance[i];
                bool spawn = UnityEngine.Random.Range(0f, 1f) < chance;
                Debug.Log(spawn);
                if(spawn)
                {
                    go.AddComponent<SpawnOnDestroy>().SetPrefabToSpawn(item);

                }
            }
            GameObject[] colliders = GameObject.FindGameObjectsWithTag("PlayerOnlyCollider");
            foreach (var item in colliders)
            {
                Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), item.GetComponent<BoxCollider2D>());
                Physics2D.IgnoreCollision(go.GetComponent<CircleCollider2D>(), item.GetComponent<BoxCollider2D>());
            }
        }
    }
}
