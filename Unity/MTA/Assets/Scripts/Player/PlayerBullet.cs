using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] public int damage;
    [SerializeField] float flyTime;
    public ParticleSystem splashVFX;

    private Rigidbody2D bulletRB;
    private GameObject player;

    void Start() 
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletRB.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        bulletRB.velocity = transform.up * bulletSpeed; 
        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f); 
        DestroyBullet(flyTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        bool enemyIsDead = false;

        EnemyHealth enemyHealthScript = other.GetComponent<EnemyHealth>();
        if (enemyHealthScript != null)
        {
            if (enemyHealthScript.enemyHealth > 0)
            {
                enemyHealthScript.DamageEnemy(damage);
            }
            else
            {
                enemyIsDead = true;
            }
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
            // Debug.Log(other.name + " " + other.transform.parent.name);
            DestroyBullet(0f);
        }
    }

    public void IncreaseDamage()
    {
        damage++;
    }

    public void EnableDoubleDamage(float time)
    {
        damage = damage * 2;
        Invoke(nameof(DisableDoubleDamage), time);
    }

    private void DisableDoubleDamage()
    {
        damage = damage / 2;
    }

    public void DisableExtraDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<Shoot>().bulletPrefab.name.Contains("Bullet"))
        {
            damage = 1;
        }
        else if (player.GetComponent<Shoot>().bulletPrefab.name.Contains("Fireball"))
        {
            damage = 2;
        }
    }

    private void PlaySplashVFX()
    {
        ParticleSystem vfx = Instantiate(splashVFX, this.transform.position, Quaternion.identity);
        vfx.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        vfx.Play();
        Destroy(vfx.gameObject, splashVFX.main.duration);
    }

    private void DestroyBullet(float waitTime)
    {
        Invoke(nameof(PlaySplashVFX), waitTime);
        Destroy(gameObject, waitTime);
    }
}
