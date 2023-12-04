using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Windows;

[Serializable]
public class ItemDrop
{
    public string itemName;
    public GameObject item;
    [Range(0, 1)]
    public float dropChance;
}


public class HordeSpawner : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private string enemyTag;
    [SerializeField] private List<GameObject> enemyTypes;
    [SerializeField] private List<ItemDrop> itemDrops;
    [SerializeField] private int maxEnemiesAllowedAlive;
    public List<GameObject> enemies;

    [Header("Player Data")]
    [SerializeField] private string playerTag;
    public List<HordePlayer> alivePlayers;

    [Header("Spawning Mechanics")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    [Header("Round Data")]
    [SerializeField] private TMPro.TMP_Text CurrentRoundText;
    [SerializeField] private string formatString;
    [SerializeField] private List<string> rounds;
    [SerializeField] private float timeBetweenRounds = 3f;

    [Header("Text Objects")]
    [SerializeField] private TMPro.TMP_Text downTimeText;
    [SerializeField] private string downTimeFormatString;

    [Header("Technical")]
    [SerializeField] private float timeBetweenChecks = 0.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent OnDownTimeStarted;
    [SerializeField] private UnityEvent OnNoEnemiesLeft;
    [SerializeField] private UnityEvent OnGameOver;
    [SerializeField] private UnityEvent OnRoundStart;
    [SerializeField] private UnityEvent OnAllEnemiesSpawned;

    [SerializeField] private TMPro.TMP_Text finalScoreText;

    public int Round => currentRound;
    public int roundsLeft { 
        get
        {
            return rounds.Count - currentRound;
        } 
    }

    private int currentRound = 0;
    private bool gameOver = false;
    private bool enemiesAllSpawned = false;
    private IEnumerator checkAliveRoutine;

    public void SpawnNextRound()
    {
        enemiesAllSpawned = false;
        StopAllCoroutines();
        StartCoroutine(SpawningRoutine());
        checkAliveRoutine = CheckAliveRoutine();
        StartCoroutine(checkAliveRoutine);
    }

    public IEnumerator CheckAliveRoutine()
    {
        alivePlayers = new List<HordePlayer>(GameObject.FindObjectsOfType<HordePlayer>(true));
        while(true)
        {
            if (enemies.Count == 0 && enemiesAllSpawned) break;
            if (gameOver) break;

            enemies.RemoveAll(x => x.IsDestroyed() || !x.activeSelf);
            alivePlayers.RemoveAll(x => x.Dead);
            if (alivePlayers.Count == 0)
            {
                gameOver = true;
                OnGameOver?.Invoke();
                if (finalScoreText) finalScoreText.text = $"{Round}";
                yield return null;          
            }
            yield return new WaitForSeconds(timeBetweenChecks);
        }
        if (!gameOver)
        {
            OnNoEnemiesLeft?.Invoke();
            StartCoroutine(DownTimeRoutine());
        }
        yield return null;
    }

    public IEnumerator SpawningRoutine()
    {
        OnRoundStart?.Invoke();
        string round = rounds[currentRound++];
        if (CurrentRoundText) CurrentRoundText.text = String.Format(formatString, currentRound);

        int i = 0;
        while(i < round.Length)
        {
            int numEnemies =  enemies.Count;
            if (numEnemies < maxEnemiesAllowedAlive)
            {
                spawnEnemy(round.Substring(i++, 1));           
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        }
        enemiesAllSpawned = true;
        OnAllEnemiesSpawned?.Invoke(); 
        yield return null;
    }

    private void spawnEnemy(string type)
    {
        if (Int32.TryParse(type, out int result))
        {
            var enemyPrefab = enemyTypes[result - 1];
            var go = GameObject.Instantiate(enemyPrefab);
            go.transform.SetParent(enemyParent);
            enemies.Add(go);
            var childIndex = UnityEngine.Random.Range(0, spawnPoints.childCount);
            var spawnPoint = spawnPoints.GetChild(childIndex);
            var spawnLocation = spawnPoint.transform.position;
            var agent = go.GetComponent<NavMeshAgent>();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnLocation, out hit, float.MaxValue, NavMesh.AllAreas))
            {
                // Set the NavMeshAgent's destination to the closest valid position
                bool worked = agent.Warp(hit.position);
            }
            else
            {
                bool worked = agent.Warp(spawnLocation);
            }
            for(int i = 0; i < itemDrops.Count; i++)
            {
                var item = itemDrops[i].item;
                var chance = itemDrops[i].dropChance;
                bool spawn = UnityEngine.Random.Range(0f, 1f) < chance;
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

    public IEnumerator DownTimeRoutine()
    {
        OnDownTimeStarted?.Invoke();
        if (downTimeText == null)
            yield return new WaitForSeconds(timeBetweenRounds);
        else
        {
            float startTime = Time.time;
            while ((Time.time - startTime) < timeBetweenRounds)
            {
                SetText((timeBetweenRounds - (Time.time - startTime)));
                yield return new WaitForEndOfFrame();
            }
        }       
        if (roundsLeft > 0)
        {
            SpawnNextRound();
        }
        yield return null;
    }

    public void SetText(float seconds)
    {
        downTimeText.text = string.Format(downTimeFormatString, (int)seconds);
    }
}
