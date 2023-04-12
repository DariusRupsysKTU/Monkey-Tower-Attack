using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem rockSplashVFX;

    private UnityEvent onBulletDestroy;
    private Rigidbody2D bulletRB;

    void Start() 
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        bulletRB.velocity = transform.up * bulletSpeed; 
        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f); 
        DestroyBullet(0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        bool enemyIsDead = false;

        EnemyHealth enemyHealthScript = other.GetComponent<EnemyHealth>();
        if (enemyHealthScript != null && enemyHealthScript.enemyHealth > 0)
        {
            enemyHealthScript.DamageEnemy(damage);
        }
        else
        {
            enemyIsDead = true;
        }

        ItemHealth itemHealthScript = other.GetComponent<ItemHealth>();
        if (itemHealthScript != null)
        {
            itemHealthScript.DamageItem(damage);
        }

        if (other.transform.tag != "Player" && other.transform.tag != "PlayerBullet" && other.transform.tag != "Currency" && 
        other.transform.tag != "SpawnPoint" && other.transform.tag != "RoomTracker" && other.transform.tag != "RoomChecker" &&
        !enemyIsDead)
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
