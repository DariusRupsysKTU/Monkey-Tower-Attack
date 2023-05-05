using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();
    public bool dropRarestPossible;
    public float dropForce;

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            // checks if any of the items have bigger or equal drop chance
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            if (!dropRarestPossible)
            {
                // gives random out of possible items
                Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
                return droppedItem;
            }
            else
            {
                // gives rarest out of possible items
                Loot rarest = possibleItems[0];
                foreach (Loot item in possibleItems)
                {
                    if (item.dropChance < rarest.dropChance)
                    {
                        rarest = item;
                    }
                }
                return rarest;
            }
        }
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();
        
        if (droppedItem != null)
        {
            GameObject lootGameObject;

            // spawns item and gives a correct sprite
            lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
            lootGameObject.transform.parent = this.transform.parent.parent;
            GetPower(droppedItem, lootGameObject);
            
            // adds random force to the spawned item
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }

    private void GetPower(Loot drop, GameObject obj)
    {
        if (drop.lootName == "Heal")
        {
            obj.GetComponent<Interaction>().heal = true;
        }
        else if (drop.lootName == "Full Heal")
        {
            obj.GetComponent<Interaction>().fullHeal = true;
        }
        else if (drop.lootName == "Double Damage")
        {
            obj.GetComponent<Interaction>().doubleDamage = true;
        }
        else if (drop.lootName == "Speed Boost")
        {
            obj.GetComponent<Interaction>().speedBoost = true;
        }
        else if (drop.lootName == "Immunity")
        {
            obj.GetComponent<Interaction>().immunity = true;
        }
        else if (drop.lootName == "Coin")
        {
            obj.GetComponent<Interaction>().coin = true;
        }
    }
}
