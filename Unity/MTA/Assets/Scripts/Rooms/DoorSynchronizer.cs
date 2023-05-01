using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSynchronizer : MonoBehaviour
{
    public bool openBothDoors;

    private DoorCollider theseDoors;
    private DoorCollider connectedDoors;

    void Start()
    {
        theseDoors = this.gameObject.GetComponent<DoorCollider>();
    }

    void Update()
    {     
        if (openBothDoors)
        {
            theseDoors.doorsOpen = true;
            connectedDoors.doorsOpen = true;
        }
        else
        {
            theseDoors.doorsOpen = false;
            connectedDoors.doorsOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Doors"))
        {
            connectedDoors = other.transform.gameObject.GetComponent<DoorCollider>();
        }
    }
}
