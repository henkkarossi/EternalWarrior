using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<GameObject> enemyPool = new List<GameObject>();
    public List<GameObject> activeEnemies = new List<GameObject>();
    public int enemyAmount;

    public void Start()
    {
        CreateEnemyPool(enemyAmount);  
    }


    public GameObject CreateEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);

        newEnemy.GetComponent<EnemyController>().enemy.manager = this;

        return newEnemy;
    }

    public void CreateEnemyPool(int amount) 
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject newEnemy = CreateEnemy();

            newEnemy.SetActive(false);

            enemyPool.Add(newEnemy);
        }
    }

    public GameObject SpawnEnemy()
    {
        GameObject newEnemy = enemyPool[enemyPool.Count - 1];

        newEnemy.SetActive(true);

        enemyPool.Remove(newEnemy);
        activeEnemies.Add(newEnemy);

        return newEnemy;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Add(enemy);
        activeEnemies.Remove(enemy);
    }

    public void SpawnAll()
    {
        var spawnPoints = transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        foreach (GameObject spawn in spawnPoints)
        {
            GameObject newEnemy = SpawnEnemy();
            newEnemy.GetComponent<NavMeshAgent>().enabled = false;
            newEnemy.transform.position = spawn.transform.position;
            newEnemy.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

}
