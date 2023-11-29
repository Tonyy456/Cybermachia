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
    [SerializeField] private TMPro.TMP_Text CurrentRound;
    [SerializeField] private int maxEnemies;
    [Range(0f, 1f)]
    [SerializeField] private List<float> spawnChance;
    [SerializeField] private List<string> rounds;
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float sqrtCountGrowthConstant;
    [SerializeField] private float linearConstant;

    [Header("Lol jus tryna get this done. combined two scripts to make life easier")]
    [SerializeField] private float timeBetweenRounds = 10f;
    [SerializeField] private string enemyTag;
    [SerializeField] private TMPro.TMP_Text downTimeText;
    [Header("Insert a {0} for the seconds")]
    [SerializeField] private string formatString;
    public List<GameObject> enemies;
    public List<HordePlayer> alivePlayers;
    public UnityEvent OnDownTimeStarted;
    public UnityEvent OnUpTimeStarted;
    public UnityEvent OnNoEnemiesLeft;
    public UnityEvent OnGameOver;
    [SerializeField] private float timeBetweenChecks = 0.5f;

    public UnityEvent OnRoundStart;
    public UnityEvent OnAllEnemiesSpawned;
    private int currentRound = 0;
    private bool gameOver = false;

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

    public int Round => currentRound;

    public int roundsLeft { 
        get
        {
            return rounds.Count - currentRound;
        } 
    }
    public void Start()
    {
    }

    public IEnumerator HandleRoundSpawn()
    {
        OnRoundStart?.Invoke();
        string round = rounds[currentRound++];
        if (CurrentRound) CurrentRound.text = $"{currentRound}".PadLeft(4, '0');
        int i = 0;
        while(i < round.Length)
        {
            int numEnemies =  enemies.Count;
            if (numEnemies < maxEnemies)
            {
                spawnEnemy(round.Substring(i++, 1));           
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        }
        noMoreEnmiesToSpawn = true;
        OnAllEnemiesSpawned?.Invoke(); 
        yield return null;
    }
    public void SpawnNextRound()
    {
        noMoreEnmiesToSpawn = false;
        StartCoroutine(HandleRoundSpawn());
        StartCoroutine(EnemyDetectionRoutine());
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
            var spawnLocation = spawnPoint.transform.localPosition;
            spawnLocation.z = 0;
            go.transform.localPosition = spawnLocation;
            for(int i = 0; i < dropItems.Count; i++)
            {
                var item = dropItems[i];
                var chance = spawnChance[i];
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

    private IEnumerator detectionRoutine;
    private bool noMoreEnmiesToSpawn = false;
    public IEnumerator EnemyDetectionRoutine()
    {
        alivePlayers = new List<HordePlayer>(GameObject.FindObjectsOfType<HordePlayer>(true));
        alivePlayers = alivePlayers.FindAll(x => !x.Dead);
        while (!noMoreEnmiesToSpawn || (enemies.Count > 0 && !gameOver))
        {
            var enemies_dead = enemies.FindAll(x => x.IsDestroyed() || !x.activeSelf);
            foreach (var x in enemies_dead) enemies.Remove(x);
            var dead_players = alivePlayers.FindAll(x => x.Dead);
            foreach (var x in dead_players) alivePlayers.Remove(x);
            if (alivePlayers.Count == 0)
            {
                gameOver = true;
                OnGameOver?.Invoke();
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

        OnUpTimeStarted?.Invoke();
        if (roundsLeft > 0) SpawnNextRound();
        yield return null;
    }

    public void SetText(float seconds)
    {
        downTimeText.text = string.Format(formatString, (int)seconds);

    }
}
