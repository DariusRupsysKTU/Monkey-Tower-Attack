using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemySpawner;
    public GameObject[] enemies;
    public int maxEnemiesPerRoom;
    private GameObject enemyManager;

    public float waitTime;
    private bool spawnedBoss = false;
    public GameObject boss;

    private float topWall = 0.7f;
    private float bottomWall = -0.7f;
    private float rightWall = 1.5f;
    private float leftWall = -1.5f;

    private RoomTemplates roomTemplates;
    private List<GameObject> rooms;

    void Start()
    {
        enemyManager = GameObject.FindWithTag("EnemyManager");
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(GetRooms), waitTime);
    }

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count-1)
                {
                    GameObject currentBoss = Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    currentBoss.transform.parent = enemyManager.transform;
                    spawnedBoss = true;
                }
                else if (i != 0)
                {
                    AddEnemySpawner(rooms[i], i);
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    void AddEnemySpawner(GameObject room, int roomIndex)
    {
        //spawns EnemySpawner game object
        Vector2 roomCenter = room.transform.position;
        float randomX = Random.Range(roomCenter.x + leftWall, roomCenter.x + rightWall);
        float randomY = Random.Range(roomCenter.y + bottomWall, roomCenter.y + topWall);
        GameObject spawner = Instantiate(enemySpawner, new Vector2(randomX, randomY), Quaternion.identity);
        spawner.transform.parent = enemyManager.transform;
        EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
        
        //changer EnemySpawner script variables
        int enemyChangeInterval = rooms.Count / enemies.Length;
        int enemyIndex = roomIndex / enemyChangeInterval;
        enemySpawnerScript.enemy = enemies[enemyIndex];
        enemySpawnerScript.enemiesLeftToSpawn = Random.Range(0, maxEnemiesPerRoom + 1);
        enemySpawnerScript.timeBetweenSpawns = Random.Range(1, 3);
        int coinFlip = Random.Range(0, 2);
        enemySpawnerScript.randomSpawn = coinFlip == 1;
        enemySpawnerScript.roomCenter = roomCenter;
        enemySpawnerScript.enemyManager = enemyManager;
    }

    void GetRooms()
    {
        rooms = roomTemplates.rooms;
    }
}
