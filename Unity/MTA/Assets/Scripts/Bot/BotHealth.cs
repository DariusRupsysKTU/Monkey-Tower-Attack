using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
    private Animator anim;
    public int botHealth;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DamageBot(int amount)
    {
        botHealth -= amount;
        if (botHealth <= 0)
        {
            anim.SetTrigger("bloon1_death");
            Destroy(this.gameObject, 2f);
        }
    }
}
