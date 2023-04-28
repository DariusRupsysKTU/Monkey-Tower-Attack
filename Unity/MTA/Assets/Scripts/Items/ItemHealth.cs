using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private Animator anim;
    public Interaction interactionScript;
    public int itemHealth;

    private bool itemSpawned = false;

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

        if (itemHealth < 0)
        {
            itemHealth = 0;
        }

        if (itemHealth == 0)
        {
            if (!itemSpawned)
            {
                GetComponent<LootBag>().InstantiateLoot(transform.position);
                itemSpawned = true;
            }

            float deathTime = 0.05f;
            Invoke(nameof(DeathAnimation), deathTime);
            Destroy(this.gameObject, deathTime + 0.5f);
            //Destroy(this.gameObject);
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("Box_Break");
    }
}
