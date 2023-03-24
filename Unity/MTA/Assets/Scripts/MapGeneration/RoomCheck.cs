using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public bool roomFound = false;
    public string neededDoors = "";
    public string notNeededDoors = "";

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("RoomTracker"))
        {
            roomFound = true;

            string roomName = other.transform.parent.name;
            string roomDirections = roomName.Substring(0, roomName.IndexOf('('));
            string roomCheckName = this.transform.name.Substring(0, 1);
            // Debug.Log("Room: " + roomDirections + " found by: " + this.transform.parent.parent.parent.name);
            
            if ((roomCheckName == "T" && roomDirections.Contains('B')) ||
            (roomCheckName == "B" && roomDirections.Contains('T')) ||
            (roomCheckName == "R" && roomDirections.Contains('L')) ||
            (roomCheckName == "L" && roomDirections.Contains('R')))
            {
                neededDoors += roomCheckName;
            }

            if ((roomCheckName == "T" && !roomDirections.Contains('B')) ||
            (roomCheckName == "B" && !roomDirections.Contains('T')) ||
            (roomCheckName == "R" && !roomDirections.Contains('L')) ||
            (roomCheckName == "L" && !roomDirections.Contains('R')))
            {
                notNeededDoors += roomCheckName;
            }
        }
    }
}
