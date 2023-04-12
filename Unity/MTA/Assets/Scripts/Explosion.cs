using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null;
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForceMultiplier;
    [SerializeField] private bool explodeOnDeath;
    [SerializeField] private bool explodeOnCollision;
    [SerializeField] private GameObject particles;

    private EnemyHealth enemyHealthScript;
    private bool collidedWithPlayer = false;

    private void Start() 
    {
        enemyHealthScript = this.GetComponent<EnemyHealth>();    
    }

    private void Update() 
    {
        if ((enemyHealthScript.enemyHealth == 0 && explodeOnDeath) || collidedWithPlayer)
        {
            if (collidedWithPlayer)
            {
                enemyHealthScript.DamageEnemy(enemyHealthScript.enemyHealth);
            }

            Explode();
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log(other.transform.tag);
        if (other.transform.tag == "Player" && explodeOnCollision)
        {
            collidedWithPlayer = true;
        }    
    }

    void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    
        foreach (Collider2D coll in inExplosionRadius)
        {
            Rigidbody2D collRB = coll.GetComponent<Rigidbody2D>();
            if (collRB != null && collRB.transform.tag != "Currency")
            {
                Vector2 distanceVector = coll.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {
                    float explosionForce = explosionForceMultiplier / distanceVector.magnitude;
                    collRB.AddForce(distanceVector.normalized * explosionForce);

                    if (collRB.transform.tag == "Player")
                    {
                        collRB.GetComponent<PlayerHealth>().DamagePlayer(explosionDamage, true);
                    }
                    if (collRB.transform.tag == "Enemy")
                    {
                        collRB.GetComponent<EnemyHealth>().DamageEnemy(explosionDamage);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }
}
