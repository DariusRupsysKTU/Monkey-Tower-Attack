using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, DataPersistence
{
    public int levelNr;

    public GameObject enemySpawnerPrefab;
    public GameObject[] enemyPrefabs;
    public int maxEnemiesPerRoom;

    public float waitTime;
    private bool spawnedBoss = false;
    public GameObject bossPrefab;
    private string bossName = "BOSS";
    private string bossSpawnerName = "BossSpawner";

    public GameObject boxSpawnerPrefab;
    public GameObject boxPrefab;

    [Header("Wall distances from the room center")]
    public float topWall = 0.7f;
    public float bottomWall = -0.7f;
    public float rightWall = 1.5f;
    public float leftWall = -1.5f;

    [Header("To avoid spawning enemies into walls")]
    public float spawnCorrection;

    private RoomTemplates roomTemplates;
    private List<GameObject> rooms;

    void Start()
    {
        if(PlayerPrefs.GetInt("NextLevel") == 1)
        {
            levelNr = PlayerPrefs.GetInt("Level");
            PlayerPrefs.SetInt("LoadedLevel", 1);
        }

        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(GetRooms), waitTime);
    }

    void Update()
    {
        if (rooms != null)
        {
            if (waitTime <= 0 && spawnedBoss == false)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    GameObject parentObject = new GameObject(rooms[i].name);
                    parentObject.transform.parent = this.transform;
                    // if (i == 1) // testing spawns boss left
                    if (i == rooms.Count-1)
                    {
                        AddBoxSpawner(rooms[i], i, parentObject);
                        AddEnemySpawner(rooms[i], i, true, parentObject);
                        spawnedBoss = true;
                    }
                    else
                    {
                        AddBoxSpawner(rooms[i], i, parentObject);
                        AddEnemySpawner(rooms[i], i, false, parentObject);
                    }
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void AddEnemySpawner(GameObject room, int roomIndex, bool bossSpawner, GameObject parent)
    {
        //spawns EnemySpawner game object
        Vector2 roomCenter = room.transform.position;
        float randomX = Random.Range(roomCenter.x + leftWall + spawnCorrection, roomCenter.x + rightWall - spawnCorrection);
        float randomY = Random.Range(roomCenter.y + bottomWall + spawnCorrection, roomCenter.y + topWall - spawnCorrection);
        GameObject spawner = Instantiate(enemySpawnerPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        spawner.transform.parent = parent.transform;

        EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();

        if (roomIndex > 0)
        {
            if (bossSpawner)
            {
                spawner.name = bossSpawnerName;
                enemySpawnerScript.enemy = bossPrefab;
                EnemyHealth bossHealthScript = enemySpawnerScript.enemy.GetComponent<EnemyHealth>();
                bossHealthScript.enemyHealth = bossHealthScript.enemyHealth * levelNr;
                enemySpawnerScript.isBossSpawner = true;
                enemySpawnerScript.bossName = bossName;
            }
            else
            {
                //change EnemySpawner script variables
                int enemyChangeInterval = rooms.Count / enemyPrefabs.Length;
                int enemyIndex = roomIndex / enemyChangeInterval;
                if (enemyIndex > enemyPrefabs.Length - 1)
                {
                    enemyIndex = enemyPrefabs.Length - 1;
                }
                enemySpawnerScript.enemy = enemyPrefabs[enemyIndex];

                int enemiesWillNotSpawn = Random.Range(1, 11);
                if (enemiesWillNotSpawn == 1)
                {
                    enemySpawnerScript.enemiesLeftToSpawn = 0;
                }
                else
                {
                    enemySpawnerScript.enemiesLeftToSpawn = Random.Range(levelNr, levelNr + 2);
                }
                
                enemySpawnerScript.timeBetweenSpawns = 0.5f;
                int coinFlip = Random.Range(0, 2);
                enemySpawnerScript.randomSpawn = coinFlip == 1;
            }
        }
        
        enemySpawnerScript.roomCenter = roomCenter;
        enemySpawnerScript.spawnCorrection = spawnCorrection;
    }

    void AddBoxSpawner(GameObject room, int roomIndex, GameObject parent)
    {

        //spawns EnemySpawner game object
        Vector2 roomCenter = room.transform.position;
        float randomX = Random.Range(roomCenter.x + leftWall + spawnCorrection, roomCenter.x + rightWall - spawnCorrection);
        float randomY = Random.Range(roomCenter.y + bottomWall + spawnCorrection, roomCenter.y + topWall - spawnCorrection);
        GameObject spawner = Instantiate(boxSpawnerPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        spawner.transform.parent = parent.transform;
        
        if (roomIndex > 0)
        {
            BoxSpawner boxSpawnerScript = spawner.GetComponent<BoxSpawner>();

            //change BoxSpawner script variables
            boxSpawnerScript.boxPrefab = boxPrefab;
            boxSpawnerScript.boxesLeftToSpawn = Random.Range(0, levelNr + 1);
            boxSpawnerScript.timeBetweenSpawns = 0.5f;
            int coinFlip = Random.Range(0, 2);
            boxSpawnerScript.randomSpawn = coinFlip == 1;
            
            boxSpawnerScript.roomCenter = roomCenter;
            boxSpawnerScript.spawnCorrection = spawnCorrection;
        }
    }

    void GetRooms()
    {
        rooms = roomTemplates.rooms;
    }

    public void LoadData(GameData data)
    {
        this.levelNr = data.level;
    }

    public void SaveData(ref GameData data)
    {
        data.level = this.levelNr;
    }
}
