using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoundDetector : MonoBehaviour
{
    [SerializeField] private HordeSpawner HordeSpawner;
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

    private bool gameOver = false;
    public void Start()
    {
        if (HordeSpawner == null) return;
    }

    public void CheckAllEnemiesDied()
    {
        StartCoroutine(EnemyDetectionRoutine());
    }

    public IEnumerator EnemyDetectionRoutine()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
        alivePlayers = new List<HordePlayer>(GameObject.FindObjectsOfType<HordePlayer>(true));
        alivePlayers = alivePlayers.FindAll(x => !x.Dead);
        while (enemies.Count > 0 && !gameOver)
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
        if(downTimeText == null)
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
        if (HordeSpawner.roundsLeft > 0) HordeSpawner.SpawnNextRound();
        yield return null;
    }

    public void SetText(float seconds)
    {
        downTimeText.text = string.Format(formatString, (int)seconds);

    }
}
