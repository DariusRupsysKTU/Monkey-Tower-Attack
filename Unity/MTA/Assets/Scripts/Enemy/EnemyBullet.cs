using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    public int damage;
    [SerializeField] bool destroyAfterTime;
    [SerializeField] float bulletTime;
    public ParticleSystem rockSplashVFX;

    [SerializeField] private AudioSource bulletSound;

    public Vector2 shootAngle;

    private UnityEvent onBulletDestroy;
    private Rigidbody2D bulletRB;

    private Vector2 playerPos;
    private Vector2 startPos;

    void Start() 
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        bulletRB.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        startPos = this.transform.position;

        bulletSound.Play();

        if (shootAngle != Vector2.zero)
        {
            float bulletRotation = Mathf.Atan2(shootAngle.y, shootAngle.x) * 180 / Mathf.PI;
            bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + bulletRotation + 90f); 
        
            bulletRB.velocity = shootAngle * bulletSpeed;
        }
        else
        {
            float bulletRotation = Mathf.Atan2((playerPos - startPos).normalized.y, (playerPos - startPos).normalized.x) * 180 / Mathf.PI;
            bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + bulletRotation + 90f); 

            // gets direction of the player and multiplies by bullet speed
            bulletRB.velocity = (playerPos - startPos).normalized * bulletSpeed; 
        }

        // bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f); 
        
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
            playerHealth.DamagePlayer(damage);
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
        vfx.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        vfx.Play();
        Destroy(vfx.gameObject, rockSplashVFX.main.duration);
    }

    public void DestroyBullet(float waitTime)
    {
        Invoke(nameof(PlayRockSplashVFX), waitTime);
        Destroy(gameObject, waitTime);
    }
}
