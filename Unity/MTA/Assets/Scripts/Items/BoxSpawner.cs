using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public int boxesLeftToSpawn;
    public float timeBetweenSpawns;
    public bool randomSpawn;
    public Vector2 roomCenter;
    public EnemyManager enemyManagerScript;
    public bool testing;
    
    private float spawnRange = 0.1f;
    private Vector2 spawnerPosition;

    public float topWall;
    public float bottomWall;
    public float rightWall;
    public float leftWall;

    public float spawnCorrection;

    void Start() 
    {
        if (!testing)
        {
            enemyManagerScript = this.transform.parent.gameObject.GetComponent<EnemyManager>();
            topWall = enemyManagerScript.topWall - spawnCorrection;
            bottomWall = enemyManagerScript.bottomWall + spawnCorrection;
            rightWall = enemyManagerScript.rightWall - spawnCorrection;
            leftWall = enemyManagerScript.leftWall + spawnCorrection;
        }
        spawnerPosition = this.transform.position;
        StartCoroutine(nameof(SpawnBoxes));    
    }

    IEnumerator SpawnBoxes()
    {
        while (boxesLeftToSpawn > 0)
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

            GameObject currentBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
            currentBox.transform.parent = this.transform;
            boxesLeftToSpawn--;
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
