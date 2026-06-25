using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<Enemy> enemies;

    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int enemyCount = 5;
    [Range(1f, 70f)]
    [SerializeField] private float spawnRadius;

    Enemy tempEnemy = null;

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = UnityEngine.Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y;
            tempEnemy = Instantiate(enemyPrefab, transform);
            tempEnemy.gameObject.SetActive(false);
            tempEnemy.SetTarget(target);
            tempEnemy.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            StartCoroutine(SpawnEnemy(tempEnemy));
        }

    }

    float percent = 0.1f;

    IEnumerator SpawnEnemy(Enemy _enemy)
    {
        //yield return new WaitForSeconds(5f);
        tempEnemy.gameObject.SetActive(true);
        while (percent <= 1f)
        {
            percent += Time.deltaTime * (float)(1 / spawnTime);
            _enemy.transform.GetChild(0).localScale = Vector3.one * percent;
            yield return null;
        }
        Debug.Log("Spawned");
    }

    public void OnTextFieldEndEdit(string txt)
    {
        int.TryParse(txt, out enemyCount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

}
