using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topDoorRooms;
    public GameObject[] rightDoorRooms;
    public GameObject[] bottomDoorRooms;
    public GameObject[] leftDoorRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;
    [Range(5, 25)]
    public int maxRooms;
    public bool tooManyRooms;

    public float waitTime;
    private bool spawnedBoss = false;
    public GameObject boss;

    public int roomCounter = 1;
    public int waitingRooms = 4;

    void Awake() 
    {
        if (roomCounter + waitingRooms >= maxRooms)
        {
            tooManyRooms = true;
        }    
    }

    void Start()
    {
        Invoke(nameof(NotEnoughRoomsCheck), 1f);
    }

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count-1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].transform.name == "ClosedRoom")
            {
                Invoke(nameof(RestartScene), 1f);
            }
        }
    }

    void RestartScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NotEnoughRoomsCheck()
    {
        if (Mathf.Abs(maxRooms - roomCounter) > 1)
        {
            RestartScene();
        }
    }
}
