using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemySpawnerPrefab;
    public GameObject[] enemies;
    public int maxEnemiesPerRoom;

    public float waitTime;
    private bool spawnedBoss = false;
    public GameObject boss;
    private string bossName = "BOSS";
    private string bossSpawnerName = "BossSpawner";

    [Header("Wall distances from the room center")]
    public float topWall = 0.7f;
    public float bottomWall = -0.7f;
    public float rightWall = 1.5f;
    public float leftWall = -1.5f;

    private RoomTemplates roomTemplates;
    private List<GameObject> rooms;

    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(GetRooms), waitTime);
    }

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                // if (i == 1) // testing
                if (i == rooms.Count-1)
                {
                    AddEnemySpawner(rooms[i], i, true);
                    spawnedBoss = true;
                }
                else if (i != 0)
                {
                    AddEnemySpawner(rooms[i], i, false);
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    void AddEnemySpawner(GameObject room, int roomIndex, bool bossSpawner)
    {
        //spawns EnemySpawner game object
        Vector2 roomCenter = room.transform.position;
        float randomX = Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall);
        float randomY = Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall);
        GameObject spawner = Instantiate(enemySpawnerPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        spawner.transform.parent = this.transform;

        EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();

        if (bossSpawner)
        {
            spawner.name = bossSpawnerName;
            enemySpawnerScript.enemy = boss;
            enemySpawnerScript.isBossSpawner = true;
            enemySpawnerScript.bossName = bossName;
        }
        else
        {
            //change EnemySpawner script variables
            int enemyChangeInterval = rooms.Count / enemies.Length;
            int enemyIndex = roomIndex / enemyChangeInterval;
            if (enemyIndex > enemies.Length - 1)
            {
                enemyIndex = enemies.Length - 1;
            }
            enemySpawnerScript.enemy = enemies[enemyIndex];
            enemySpawnerScript.enemiesLeftToSpawn = Random.Range(0, maxEnemiesPerRoom + 1);
            enemySpawnerScript.timeBetweenSpawns = Random.Range(1, 3);
            int coinFlip = Random.Range(0, 2);
            enemySpawnerScript.randomSpawn = coinFlip == 1;
        }
        
        enemySpawnerScript.roomCenter = roomCenter;
    }

    void GetRooms()
    {
        rooms = roomTemplates.rooms;
    }
}
