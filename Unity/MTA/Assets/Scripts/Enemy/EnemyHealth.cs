using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Animator anim;
    public int enemyHealth;

    private bool itemSpawned = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DamageEnemy(int amount)
    {
        if (enemyHealth > 0)
        {
            anim.Play("bloon_hit");
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
            Destroy(this.gameObject, deathTime + 0.5f);
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("bloon1_death");
    }
}
