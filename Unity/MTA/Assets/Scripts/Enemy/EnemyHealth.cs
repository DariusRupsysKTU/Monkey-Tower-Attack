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
        enemyHealth -= amount;
        if (enemyHealth <= 0)
        {
            anim.SetTrigger("bloon1_death");
            Destroy(this.gameObject, 0.5f);
        }
        else
        {
            anim.Play("bloon_hit");
        }
    }
}
