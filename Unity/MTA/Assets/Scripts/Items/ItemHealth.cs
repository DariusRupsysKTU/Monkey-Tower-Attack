using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private Animator anim;
    public GameObject brokenBoxPrefab;
    public Interaction interactionScript;
    public int itemHealth;

    private float deathTime = 0.05f;
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

            DeathAnimation();
            Invoke(nameof(DestroyBox), 0.25f);
        }
    }

    private void DeathAnimation()
    {
        anim.SetTrigger("Box_Break");
    }

    private void DestroyBox()
    {
        GameObject brokenBox = Instantiate(brokenBoxPrefab, this.transform.position, Quaternion.identity);
        brokenBox.transform.parent = this.transform.parent;
        Destroy(this.gameObject);
    }
}
