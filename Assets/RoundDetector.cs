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
    public UnityEvent OnDownTimeStarted;
    public UnityEvent OnUpTimeStarted;
    public UnityEvent OnNoEnemiesLeft;

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
        int i = 0;
        while(enemies.Count > 0)
        {
            GameObject enemy = enemies[i];
            if (enemy.IsDestroyed() || !enemy.activeSelf)
            {
                enemies.Remove(enemy);
            }
            i = enemies.Count == 0 ? -1 : i % enemies.Count;
            yield return new WaitForEndOfFrame();
        }
        OnNoEnemiesLeft?.Invoke();
        StartCoroutine(DownTimeRoutine());
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
