using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrencyPickUp : MonoBehaviour
{
    public int value = 100;
   // public Inventory inventory;

    /*private void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }*/

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Inventory.instance.IncreaseCurrency(value);
        }
    }
}
