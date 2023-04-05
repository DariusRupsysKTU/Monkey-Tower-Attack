using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Animator anim;
    public int enemyHealth;

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

        if (enemyHealth == 0)
        {
            int score = PlayerPrefs.GetInt("Score") + 100;
            PlayerPrefs.SetInt("Score", score);

            GetComponent<LootBag>().InstantiateLoot(transform.position);

            float deathTime = 0.05f;
            Invoke(nameof(DeathAnimation), deathTime);
            Destroy(this.gameObject, deathTime + 0.5f);
        }
    }

    private void DeathAnimation()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("bloon1_death");
    }
}
