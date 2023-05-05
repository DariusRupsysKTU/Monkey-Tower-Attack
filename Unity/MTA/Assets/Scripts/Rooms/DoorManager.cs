using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool doorsWithoutPlayerClose;
    public int index;
    public GameObject enemyManager;
    public EnemyManager enemyManagerScript;
    public EnemySpawner enemySpawnerScript;
    public GameObject player;
    public List<GameObject> doors;
    public int enemyCount;
    public bool playerIsInTheRoom;
    public bool roomIsClosed = false;

    public float calculateAfterTime;
    public float setCalculateAfterTime;

    private float topWallPosition;
    private float bottomWallPosition;
    private float rightWallPosition;
    private float leftWallPosition;

    void Start()
    {
        index = this.transform.GetSiblingIndex();
        enemyManager = GameObject.Find("Enemy Manager");
        enemyManagerScript = enemyManager.GetComponent<EnemyManager>();
        Invoke(nameof(GetInfo), setCalculateAfterTime);

        GetRoomDoors();

        calculateAfterTime = setCalculateAfterTime;
    }

    void Update()
    {
        if (calculateAfterTime <= 0)
        {
            ManageDoors(doorsWithoutPlayerClose);
            calculateAfterTime = setCalculateAfterTime;
            calculateAfterTime = 0;
        }
        else
        {
            calculateAfterTime -= Time.deltaTime;
        }
        
    }

    private void ManageDoors(bool closeWhenNoPlayer)
    {
        EnemyInRoomCount();
        PlayerIsInTheRoom(player.transform.position);

        if (playerIsInTheRoom && enemyCount == 0)
        {
            // Debug.Log("opening " + enemyManager.transform.GetChild(index).name);
            OpenRoomDoors();
            roomIsClosed = false;
        }
        else if (closeWhenNoPlayer && !playerIsInTheRoom && !roomIsClosed)
        {
            // Debug.Log("closing " + enemyManager.transform.GetChild(index).name);
            CloseRoomDoors();
            roomIsClosed = true;
        }
        else if (playerIsInTheRoom && enemyCount != 0)
        {
            // Debug.Log("closing " + enemyManager.transform.GetChild(index).name);
            CloseRoomDoors();
            roomIsClosed = true;
        }
    }

    private void GetInfo()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySpawnerScript = enemyManager.transform.GetChild(index).GetChild(1).gameObject.GetComponent<EnemySpawner>();
        GetWallPositions();
    }

    private void EnemyInRoomCount()
    {
        Transform enemyManagerOfRoom = enemyManager.transform.GetChild(index);
        if (enemyManagerOfRoom.childCount > 0)
        {
            GameObject enemyHolder = enemyManagerOfRoom.GetChild(1).gameObject;
            enemyCount = enemyHolder.transform.childCount;
        }
        else
        {
            enemyCount = 0;
        }
    }

    private void GetRoomDoors()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.name.Contains("Doors") && !doors.Contains(child.gameObject))
            {
                doors.Add(child.gameObject);
            }
        }
    }

    private void OpenRoomDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorSynchronizer thisSynchronizer;
            LocateSynchronizer thisLocator;

            if (door.TryGetComponent<DoorSynchronizer>(out thisSynchronizer))
            {
                thisSynchronizer.openBothDoors = true;
            }
            if (door.TryGetComponent<LocateSynchronizer>(out thisLocator))
            {
                thisLocator.doorsWithSynchronizer.GetComponent<DoorSynchronizer>().openBothDoors = true;
            }
        }
    }

    private void CloseRoomDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorSynchronizer thisSynchronizer;
            LocateSynchronizer thisLocator;

            if (door.TryGetComponent<DoorSynchronizer>(out thisSynchronizer))
            {
                thisSynchronizer.openBothDoors = false;
            }
            if (door.TryGetComponent<LocateSynchronizer>(out thisLocator))
            {
                thisLocator.doorsWithSynchronizer.GetComponent<DoorSynchronizer>().openBothDoors = false;
            }
        }
    }

    private void GetWallPositions()
    {
        topWallPosition = enemySpawnerScript.roomCenter.y + enemyManagerScript.topWall;
        bottomWallPosition = enemySpawnerScript.roomCenter.y + enemyManagerScript.bottomWall;
        rightWallPosition = enemySpawnerScript.roomCenter.x + enemyManagerScript.rightWall;
        leftWallPosition = enemySpawnerScript.roomCenter.x + enemyManagerScript.leftWall;
    }

    private void PlayerIsInTheRoom(Vector2 targetPosition)
    {
        if (topWallPosition != 0)
        {
            if (targetPosition.x >= leftWallPosition && targetPosition.x <= rightWallPosition &&
                targetPosition.y >= bottomWallPosition && targetPosition.y <= topWallPosition)
            {
                playerIsInTheRoom = true;
            }
            else
            {
                playerIsInTheRoom = false;
            }
        }
    }
}