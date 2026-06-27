using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform mainCameraOffset;
    [SerializeField] private Transform target;

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<Enemy> enemies;

    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnRadius;

    [SerializeField] private int enemiesAlive = 0;
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private Slider powerUpBar;
    [SerializeField] private Image powerUpColor;
    [SerializeField] Color[] powerUpColors;

    int MIN_ENEMY_COUNT = 20;
    int SPAWN_THRESHOLD = 2;

    Enemy tempEnemy = null;

    float WAVE_SIZE_PERCENT = 0.8f;

    Queue<Enemy> enemyPool = null;

    Color currentWaveColor;
    int currentWaveIdx = -1;

    int powerThreshold = 0;

    public void FakeStart()
    {
        enemyPool = new Queue<Enemy>();
        currentWaveColor = powerUpColors[GetCurrentWaveIdx()];
        Vector3 offsetPos = mainCameraOffset.position;
        for (int i = 0; i < enemyCount; i++)
        {
            tempEnemy = Instantiate(enemyPrefab, transform);
            tempEnemy.gameObject.SetActive(false);
            tempEnemy.OnDeathTrigger.AddListener(WhenEnemyDie);
            enemyPool.Enqueue(tempEnemy);
            tempEnemy.name = $"Enemy_{i:D2}";
            tempEnemy.Target = target;
            tempEnemy.CameraPosition = offsetPos;
            tempEnemy.DefaultColor = currentWaveColor;
        }

        tempEnemy = null;

        enemiesKilled = 0;
        scoreTxt.text = enemiesKilled.ToString("D4");
        powerThreshold = (int)(enemyCount * 0.75f);
        powerUpBar.value = 0;
        powerUpColor.color = currentWaveColor;

        SpawnWave();
    }

    int GetCurrentWaveIdx()
    {
        currentWaveIdx = (currentWaveIdx + 1) % powerUpColors.Length;
        return currentWaveIdx;
    }

    private void WhenEnemyDie(Enemy _enemy)
    {
        enemyPool.Enqueue(_enemy);
        enemiesKilled += 1;
        enemiesAlive -= 1;
        scoreTxt.text = enemiesKilled.ToString("D4");
        powerUpBar.value = (float)(enemiesKilled % powerThreshold) / powerThreshold;
        if ((enemiesKilled % powerThreshold) == 0)
        {
            currentWaveColor = powerUpColors[GetCurrentWaveIdx()];
            powerUpColor.color = currentWaveColor;
        }
        if (enemiesAlive < SPAWN_THRESHOLD)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < (WAVE_SIZE_PERCENT * enemyCount); i++)
        {
            StartCoroutine(SpawnEnemy(enemyPool.Dequeue()));
            enemiesAlive += 1;
        }
    }

    IEnumerator SpawnEnemy(Enemy _enemy)
    {
        Vector3 _spawnPosition = target.position + UnityEngine.Random.insideUnitSphere * Mathf.Clamp( enemyCount, 20, spawnRadius);
        yield return null;
        _spawnPosition.y = transform.position.y;
        _enemy.transform.SetPositionAndRotation(_spawnPosition, Quaternion.identity);
        _enemy.DefaultColor = currentWaveColor;
        _enemy.transform.GetChild(0).localScale = Vector3.one * UnityEngine.Random.Range(0.89f, 1.4f);
        _enemy.gameObject.SetActive(true);        
    }

    public void OnTextFieldEndEdit(string txt)
    {
        int.TryParse(txt, out enemyCount);

        enemyCount = (enemyCount <= 0) ? MIN_ENEMY_COUNT : enemyCount;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

}
