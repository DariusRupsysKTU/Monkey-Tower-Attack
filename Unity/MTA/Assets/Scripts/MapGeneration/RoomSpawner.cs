using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour    //this script is on every SpawnPoint
{
    public int roomSpawnDirection;
    // 1 --> spawn on top, bottom door
    // 2 --> spawn on right, left door
    // 3 --> spawn on bottom, top door
    // 4 --> spawn on left, right door

    public GameObject roomCheckers;         //every SpawnPoint has RoomCheckers to all four directions 
                                            //to check what rooms are near by before spawning new room

    private RoomTemplates templates;        //script that holds information about different rooms and values needed for room spawning
    private int randomNum;                  //random index that will be used to pick room from array
    public bool spawned = false;            //value showing if this SpawnPoint has spawned the room
    private GameObject parentGameObject;    //object (Grid) under which every room is being spawned

    public float waitTime;                  //time before destroying this SpawnPoint

    string spawnPointLetter1;               //First letter of the SpawnPoint name showing which SpawnPoint is this
    string neededDoors;                     //Value made out of letters showing to which directions doors are needed
    string notNeededDoors;                  //Value made out of letters showing to which directions doors are not needed

    void Start()
    {
        spawnPointLetter1 = this.name.Substring(0, 1);
        neededDoors = GetOppositeSideLetter(spawnPointLetter1);
        notNeededDoors = "";

        Destroy(gameObject, waitTime);      // Destroys spawn point for performance optimization
        parentGameObject = GameObject.FindWithTag("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke(nameof(SpawnRoom), 0.1f);
    }

    /*
    * Method that spawns room on the SpawnPoint
    */
    void SpawnRoom()    
    {
        if (spawned == false)
        {
            CheckRooms();

            GameObject room = null;
            if (roomSpawnDirection == 1)
            {
                GetCorrectRoomIndex(templates.bottomDoorRooms, neededDoors, notNeededDoors);
                room = Instantiate(GetGoodRoomArray(templates.bottomDoorRooms, neededDoors, notNeededDoors)[randomNum], transform.position, Quaternion.identity);
            }
            else if (roomSpawnDirection == 2)
            {
                GetCorrectRoomIndex(templates.leftDoorRooms, neededDoors, notNeededDoors);
                room = Instantiate(GetGoodRoomArray(templates.leftDoorRooms, neededDoors, notNeededDoors)[randomNum], transform.position, Quaternion.identity);
            } 
            else if (roomSpawnDirection == 3)
            {
                GetCorrectRoomIndex(templates.topDoorRooms, neededDoors, notNeededDoors);
                room = Instantiate(GetGoodRoomArray(templates.topDoorRooms, neededDoors, notNeededDoors)[randomNum], transform.position, Quaternion.identity);
            }
            else if (roomSpawnDirection == 4)
            {
                GetCorrectRoomIndex(templates.rightDoorRooms, neededDoors, notNeededDoors);
                room = Instantiate(GetGoodRoomArray(templates.rightDoorRooms, neededDoors, notNeededDoors)[randomNum], transform.position, Quaternion.identity);
            }

            room.transform.parent = parentGameObject.transform;
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        //checks if this SpawnPoint collided with other room SpawnPoint
        if (other.CompareTag("SpawnPoint"))     
        {
            //in collision place puts closedRoom/blackRoom 
            try
            {
                if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false && !transform.position.Equals(Vector2.zero))
                {
                    // Debug.Log("collision " + this.gameObject.transform.parent.name + " " + templates.closedRoom.name);
                    GameObject room = Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    room.transform.parent = parentGameObject.transform;
                    Destroy(gameObject);
                }
            }
            catch {}

            spawned = true;
        }
    }

    /*
    * According to good and bad directions gets random (or not) room index
    */
    private void GetCorrectRoomIndex(GameObject[] rooms, string goodDirections, string badDirections)
    {
        if (!templates.tooManyRooms)
        {
            if (templates.roomCounter + templates.waitingRooms >= templates.maxRooms)
            {
                templates.tooManyRooms = true;
            }

            List<GameObject> goodRooms = GetGoodRoomArray(rooms, goodDirections, badDirections);
            if (goodRooms.Count == 0)
            {
                randomNum = 0;
            }
            else
            {
                randomNum = Random.Range(0, goodRooms.Count);
            }

            // Debug.Log("(going to put: " + goodRooms[randomNum].transform.name + ") (good: " + goodDirections + ") (bad: " + badDirections + ") (existing rooms: " + templates.roomCounter + ") (waiting rooms: " + templates.waitingRooms + ") (too many? " + templates.tooManyRooms + ")");
            if (goodRooms[randomNum].transform.name.Length == goodDirections.Length)
            {
                templates.waitingRooms -= goodDirections.Length;
            }
            else
            {
                templates.waitingRooms += goodRooms[randomNum].transform.name.Length - goodDirections.Length - 1;
            }
        }
        else
        {
            randomNum = 0;
            // Debug.Log("(going to put: " + rooms[randomNum].transform.name + ") (good: " + goodDirections + ") (bad: " + badDirections + ") (existing rooms: " + templates.roomCounter + ") (waiting rooms: " + templates.waitingRooms + ") (too many? " + templates.tooManyRooms + ")");
            templates.waitingRooms -= 1;
        }

        templates.roomCounter++;
    }

    /*
    * Finds every possible room that fits in with good and bad door directions
    */
    private List<GameObject> GetGoodRoomArray(GameObject[] rooms, string goodDirections, string badDirections)
    {
        List<GameObject> goodRooms = new List<GameObject>();

        if (templates.tooManyRooms && goodDirections.Length == 1)
        {
            goodRooms.Add(rooms[0]);
        }
        else
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                string roomName = rooms[i].transform.name;
                if (badDirections == "")
                {
                    badDirections = ".";
                }

                if (roomName.Contains(badDirections))
                {
                    continue;
                }
                else if (roomName.Contains(goodDirections))
                {
                    goodRooms.Add(rooms[i]);
                }
            }
        }

        return goodRooms;
    }

    /*
    * Get correct side of rooms
    */
    private GameObject[] GetCorrectSideRooms()
    {
        if (roomSpawnDirection == 1)
            return templates.bottomDoorRooms;
        else if (roomSpawnDirection == 2)
            return templates.leftDoorRooms;
        else if (roomSpawnDirection == 3)
            return templates.topDoorRooms;
        else if (roomSpawnDirection == 4)
            return templates.rightDoorRooms;
        else
            return null;
    }

    /*
    * Returns opposite side letter 
    * (T = top)
    * (B = bot)
    * (R = right)
    * (L = left)
    */
    private string GetOppositeSideLetter(string letter)
    {
        string oppositeSideLetter = "";
        if (letter == "T")
        {
            oppositeSideLetter = "B";
        }
        else if (letter == "B")
        {
            oppositeSideLetter = "T";
        }
        else if (letter == "R")
        {
            oppositeSideLetter = "L";
        }
        else if (letter == "L")
        {
            oppositeSideLetter = "R";
        }
        return oppositeSideLetter;
    }

    /*
    * Goes through all RoomCheckers and finds all good and bad door directions
    */
    private void CheckRooms()
    {
        for (int i = 0; i < roomCheckers.transform.childCount; i++)
        {
            GameObject roomCheck = roomCheckers.transform.GetChild(i).gameObject;
            neededDoors = AddDirection(neededDoors, roomCheck.GetComponent<RoomCheck>().neededDoors);
            notNeededDoors = AddDirection(notNeededDoors, roomCheck.GetComponent<RoomCheck>().notNeededDoors);
        }
    }

    /*
    * Adds letter (dirrection) to existing letters if this letter doesn't already exist
    */
    private string AddDirection(string current, string directionLetter)
    {
        if (!current.Contains(directionLetter))
        {
            current += directionLetter;
        }
        return current;
    }
}
