using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableControler : MonoBehaviour
{
    public bool isPickUp;
    public bool interactionSpent;
    public void Interact()
    {
        if (!interactionSpent)
        {
            interactionSpent = true;
            Debug.Log("interaction happened");
        }
        else
        {
            Debug.Log("object is spent");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactionSpent && isPickUp)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
