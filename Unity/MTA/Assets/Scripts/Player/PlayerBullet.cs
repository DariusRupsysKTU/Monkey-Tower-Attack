using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBullet : MonoBehaviour, DataPersistence
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem rockSplashVFX;

    private Rigidbody2D bulletRB;
    private GameObject player;

    void Start() 
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletRB.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        bulletRB.velocity = transform.up * bulletSpeed; 
        Debug.Log(transform.up);
        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 180f); 
        DestroyBullet(0.5f);
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
        damage = 1;
    }

    private void PlayRockSplashVFX()
    {
        ParticleSystem vfx = Instantiate(rockSplashVFX, this.transform.position, Quaternion.identity);
        vfx.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        vfx.Play();
        Destroy(vfx.gameObject, rockSplashVFX.main.duration);
    }

    private void DestroyBullet(float waitTime)
    {
        Invoke(nameof(PlayRockSplashVFX), waitTime);
        Destroy(gameObject, waitTime);
    }

    public void LoadData(GameData data)
    {
        this.damage = data.bulletDamage;
    }

    public void SaveData(ref GameData data)
    {
        data.bulletDamage = this.damage;
    }
}
