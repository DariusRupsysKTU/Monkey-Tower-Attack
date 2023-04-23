using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Animator anim;
    public bool isBoss;
    public int enemyHealth;

    public bool bossEnraged = false;

    [SerializeField] private AudioSource enemyDeathSound;

    private int startHealth;
    private bool itemSpawned = false;

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

        if (enemyHealth > 0 && bossEnraged)
        {
            anim.Play("boss_hit");
        }

        if (enemyHealth <= startHealth / 2)
        {
            anim.SetBool("isEnraged", true);
            bossEnraged = true;
        }

        enemyHealth -= amount;

        if (enemyHealth < 0)
        {
            enemyHealth = 0;
        }

        if (enemyHealth == 0)
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            int score = PlayerPrefs.GetInt("Score") + 100;
            PlayerPrefs.SetInt("Score", score);

            if (!itemSpawned)
            {
                GetComponent<LootBag>().InstantiateLoot(transform.position);
                itemSpawned = true;
            }

            float deathTime = 0.05f;
            Invoke(nameof(DeathAnimation), deathTime);
            enemyDeathSound.Play();
            Destroy(this.gameObject, deathTime + 0.5f);
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("bloon1_death");
    }
}
