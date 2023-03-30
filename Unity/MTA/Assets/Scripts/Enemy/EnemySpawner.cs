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
    public GameObject enemyManager;
    
    private float spawnRange = 0.1f;
    private Vector2 spawnerPosition;

    private float topWall = 0.7f;
    private float bottomWall = -0.7f;
    private float rightWall = 1.5f;
    private float leftWall = -1.5f;

    void Start() 
    {
        spawnerPosition = this.transform.position;
        StartCoroutine(nameof(SpawnEnemies));    
    }

    IEnumerator SpawnEnemies()
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
            currentEnemy.transform.parent = enemyManager.transform;
            enemiesLeftToSpawn--;
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
