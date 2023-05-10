using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFireball : MonoBehaviour, DataPersistence
{
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem fireVFX;

    private Rigidbody2D bulletRB;

    void Start()
    {
        bulletRB = this.GetComponent<Rigidbody2D>();
        bulletRB.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        bulletRB.velocity = transform.up * bulletSpeed;
        bulletRB.transform.eulerAngles = new Vector3(0f, 0f, bulletRB.transform.eulerAngles.z + 90f);
        DestroyBullet(5.0f);
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

    private void PlayFireVFX()
    {
        ParticleSystem vfx = Instantiate(fireVFX, this.transform.position, Quaternion.identity);
        vfx.transform.localScale = new Vector3(1f + (damage * 0.1f), 1f + (damage * 0.1f), 1f + (damage * 0.1f));
        vfx.Play();
        Destroy(vfx.gameObject, fireVFX.main.duration);
    }

    // private void AddBlast()
    // {
    //     GameObject blast = Instantiate(blastPrefab, this.transform.position, Quaternion.identity);
    //     blast.GetComponent<BlastWave>().goOnStart = true;
    //     blast.GetComponent<BlastWave>().damageEnemy = true;
    //     Destroy(blast.gameObject, 2f);
    // }

    private void DestroyBullet(float waitTime)
    {
        // Invoke(nameof(AddBlast), waitTime);
        Invoke(nameof(PlayFireVFX), waitTime);
        Destroy(this.gameObject, waitTime);
    }

    public void LoadData(GameData data)
    {
        this.damage = data.fireballDamage;
    }

    public void SaveData(ref GameData data)
    {
        data.fireballDamage = this.damage;
    }
}
