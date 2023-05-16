using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int enemyHealth;

    public bool isBoss;
    public bool bossEnraged = false;
    public bool lastAttack = false;

    [SerializeField] private AudioSource enemyDeathSound;

    private int startHealth;
    private bool itemSpawned = false;

    private Animator anim;

    private void Start()
    {
        startHealth = enemyHealth;
        anim = GetComponent<Animator>();
    }

    public void DamageEnemy(int amount)
    {
        if (enemyHealth > 0 && !bossEnraged)
        {
            anim.Play("bloon_hit");
        }

        if (isBoss)
        {
            if (enemyHealth > 0 && bossEnraged)
            {
                anim.Play("boss_hit");
            }

            if (enemyHealth <= startHealth / 2)
            {
                anim.SetBool("isEnraged", true);
                bossEnraged = true;
            }
        }


        enemyHealth -= amount;

        if (enemyHealth < 0)
        {
            enemyHealth = 0;
        }

        if (enemyHealth == 0)
        {
            lastAttack = true;

            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            if (isBoss)
            {
                Inventory.instance.IncreaseScore(1000);
            }
            else if (this.gameObject.name.Contains("1"))
            {
                Inventory.instance.IncreaseScore(100);
            }
            else if (this.gameObject.name.Contains("2"))
            {
                Inventory.instance.IncreaseScore(250);
            }
            else if (this.gameObject.name.Contains("3"))
            {
                Inventory.instance.IncreaseScore(75);
            }
            else
            {
                Inventory.instance.IncreaseScore(200);
            }

            if (!itemSpawned)
            {
                GetComponent<LootBag>().InstantiateLoot(transform.position);
                itemSpawned = true;
            }

            float deathTime = 0.05f;
            Invoke(nameof(DeathAnimation), deathTime);
            Destroy(this.gameObject, deathTime + 0.5f);
            enemyDeathSound.Play();  
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("bloon1_death");
    }
}
