using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] private float minRoundDelay;
    [SerializeField] private float maxRoundDelay;

    public void Start()
    {
        SpawnNextRound();
    }

    public IEnumerator HandleRoundSpawn()
    {
        if (controlString.Length == 0) yield return null;
        List<string>  parts = new List<string>(controlString.Split('+'));      
        for (int roundIndex = 0; roundIndex < parts.Count; roundIndex++)
        {
            string round = parts[roundIndex];
            for (int i = 0; i < round.Length; i++)
            {
                spawnEnemy(round.Substring(i, 1));
                yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minRoundDelay, maxRoundDelay));
        }
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
