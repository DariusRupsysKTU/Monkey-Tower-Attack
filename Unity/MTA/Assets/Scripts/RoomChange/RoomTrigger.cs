using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Vector3 cameraChange;

    public Vector3 playerChange;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            TransitionRoom(other);
        }    
    }

    private void TransitionRoom(Collider2D other)
    {    
        cam.transform.position += cameraChange;
        other.transform.position += playerChange;
    }
}
