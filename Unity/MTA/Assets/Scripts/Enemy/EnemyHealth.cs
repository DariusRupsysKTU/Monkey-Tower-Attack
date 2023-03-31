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

        if (enemyHealth <= 0)
        {
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
