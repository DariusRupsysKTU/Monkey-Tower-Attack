using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int enemiesLeftToSpawn;
    public float timeBetweenSpawns;
    public bool randomSpawn;
    public Vector2 roomCenter;
    public EnemyManager enemyManagerScript;
    public bool isBossSpawner;
    public string bossName;
    public bool testing;
    
    private float spawnRange = 0.1f;
    private Vector2 spawnerPosition;

    public float topWall;
    public float bottomWall;
    public float rightWall;
    public float leftWall;

    public float spawnCorrection;

    private bool itemSpawned = false;

    void Start() 
    {
        if (!testing)
        {
            enemyManagerScript = this.transform.parent.parent.gameObject.GetComponent<EnemyManager>();
            topWall = enemyManagerScript.topWall - spawnCorrection;
            bottomWall = enemyManagerScript.bottomWall + spawnCorrection;
            rightWall = enemyManagerScript.rightWall - spawnCorrection;
            leftWall = enemyManagerScript.leftWall + spawnCorrection;
        }
        spawnerPosition = this.transform.position;
        StartCoroutine(nameof(SpawnEnemies));    
    }

    void Update()
    {
        if (isBossSpawner && this.gameObject.transform.childCount == 0)
        {
            if (!itemSpawned)
            {
                GetComponent<LootBag>().InstantiateLoot(transform.position);
                itemSpawned = true;
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        if (isBossSpawner)
        {
            GameObject boss = Instantiate(enemy, roomCenter, Quaternion.identity);
            boss.transform.parent = this.transform;
            boss.name = bossName;
        }
        else
        {
            while (enemiesLeftToSpawn > 0)
            {
                Vector2 spawnPosition;

                if (randomSpawn)
                {
                    float randomWidth = Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall);
                    float randomHeight = Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall);
                    spawnPosition = new Vector2(randomWidth, randomHeight);
                }
                else
                {
                    float xPosition = Random.Range(spawnerPosition.x - spawnRange, spawnerPosition.x + spawnRange);
                    float yPosition = Random.Range(spawnerPosition.y - spawnRange, spawnerPosition.y + spawnRange);
                    spawnPosition = new Vector2(xPosition, yPosition);
                }

                GameObject currentEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
                currentEnemy.transform.parent = this.transform;
                if (currentEnemy.name == "Enemy1(Clone)")
                {
                    currentEnemy.GetComponent<Enemy1Movement>().enabled = true;
                }
                enemiesLeftToSpawn--;
                
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
}
