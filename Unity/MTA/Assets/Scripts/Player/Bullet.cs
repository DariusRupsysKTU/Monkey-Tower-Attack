using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D bulletRB;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;

    void Start() 
    {
        bulletRB.velocity = transform.up * bulletSpeed; 
        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f); 
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        BotHealth botHealth = other.GetComponent<BotHealth>();
        if (botHealth != null)
        {
            botHealth.DamageBot(damage);
        }

        if (other.transform.tag != "Player" && other.transform.tag != "Bullet" && other.transform.tag != "Currency")
        {
            Destroy(gameObject);
        }
    }
}
