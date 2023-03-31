using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private Animator anim;
    public Interaction interactionScript;
    public int itemHealth;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DamageItem(int amount)
    {
        if (!interactionScript.isPickUp)
        {
            itemHealth -= amount;
        }

        if (itemHealth <= 0)
        {
            // float deathTime = 0.05f;
            // Invoke(nameof(DeathAnimation), deathTime);
            // Destroy(this.gameObject, deathTime + 0.5f);
            Destroy(this.gameObject);
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("bloon1_death");
    }
}
