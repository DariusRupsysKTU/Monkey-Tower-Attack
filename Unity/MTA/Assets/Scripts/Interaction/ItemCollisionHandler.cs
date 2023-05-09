using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    private Interaction interactionScript;
    private CircleCollider2D thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<CircleCollider2D>();
        thisCollider.isTrigger = true;
        interactionScript = this.transform.parent.GetComponent<Interaction>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && interactionScript.isPickUp)
        {
            this.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            thisCollider.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player" && interactionScript.isPickUp)
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player" && interactionScript.isPickUp)
        {
            this.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            thisCollider.isTrigger = true;
        }
    }
}
