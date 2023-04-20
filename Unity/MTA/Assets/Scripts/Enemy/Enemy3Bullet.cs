using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy3Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
    [SerializeField] bool destroyAfterTime;
    [SerializeField] float bulletTime;
    [SerializeField] ParticleSystem rockSplashVFX;

    public Vector2 shootAngle;

    private UnityEvent onBulletDestroy;
    private Rigidbody2D bulletRB;

    private Vector2 playerPos;
    private Vector2 startPos;

    void Start() 
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        startPos = this.transform.position;

        if (shootAngle != Vector2.zero)
        {
            bulletRB.velocity = shootAngle * bulletSpeed;
        }
        else
        {
            // gets direction of the player and multiplies by bullet speed
            bulletRB.velocity = (playerPos - startPos).normalized * bulletSpeed; 
        }

        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f); 
        
        if (destroyAfterTime)
        {
            DestroyBullet(bulletTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.DamagePlayer(damage, false);
        }

        ItemHealth itemHealth = other.GetComponent<ItemHealth>();
        if (itemHealth != null)
        {
            itemHealth.DamageItem(damage);
        }

        if (other.transform.tag != "Enemy" && other.transform.tag != "EnemyBullet" && other.transform.tag != "Currency" && 
        other.transform.tag != "SpawnPoint" && other.transform.tag != "RoomTracker" && other.transform.tag != "RoomChecker")
        {
            DestroyBullet(0f);
        }
    }

    private void PlayRockSplashVFX()
    {
        ParticleSystem vfx = Instantiate(rockSplashVFX, this.transform.position, Quaternion.identity);
        vfx.Play();
        Destroy(vfx.gameObject, rockSplashVFX.main.duration);
    }

    private void DestroyBullet(float waitTime)
    {
        Invoke(nameof(PlayRockSplashVFX), waitTime);
        Destroy(gameObject, waitTime);
    }
}
