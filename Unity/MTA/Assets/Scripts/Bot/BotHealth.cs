using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
    private Animator anim;
    public int botHealth;

    public void DamageBot(int amount)
    {
        botHealth -= amount;

        if (botHealth <= 0)
        {
            //Destroy(this.gameObject);
            anim.SetTrigger("bloon1_death");
        }
    }
}
