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
    [SerializeField] private List<GameObject> ignoreCollisions;
    [SerializeField] private List<GameObject> enemyTypes;
    [SerializeField] private string controlString;
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    public UnityEvent OnRoundStart;
    public UnityEvent OnAllEnemiesSpawned;
    private int currentRound = 0;
    private List<string> rounds;

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
            foreach (var item in ignoreCollisions)
            {
                Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), item.GetComponent<BoxCollider2D>());
                Physics2D.IgnoreCollision(go.GetComponent<CircleCollider2D>(), item.GetComponent<BoxCollider2D>());
            }
        }
    }
}
