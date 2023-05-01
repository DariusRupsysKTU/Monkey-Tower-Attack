using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorCollider : MonoBehaviour
{
    public bool doorsOpen;

    private void Start() 
    {
        doorsOpen = false;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, doorsOpen);
        }
    }
}
