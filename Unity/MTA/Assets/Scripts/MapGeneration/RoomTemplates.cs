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
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].transform.name == "ClosedRoom" || rooms[i].transform.name == "ClosedRoom(Clone)")
            {
                RestartScene();
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
