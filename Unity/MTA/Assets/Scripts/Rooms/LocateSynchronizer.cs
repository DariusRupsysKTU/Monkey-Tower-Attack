using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateSynchronizer : MonoBehaviour
{
    public GameObject doorsWithSynchronizer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Doors"))
        {
            doorsWithSynchronizer = other.transform.gameObject;
        }
    }
}
