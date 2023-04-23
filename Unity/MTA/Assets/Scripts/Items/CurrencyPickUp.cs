using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrencyPickUp : MonoBehaviour
{
    public int value = 100;
    private int score = 0;

    [SerializeField] private AudioSource coinPickUp;
    // public Inventory inventory;

    /*private void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }*/

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            coinPickUp.Play();

            Destroy(gameObject);
            Inventory.instance.IncreaseCurrency(value);
            score = PlayerPrefs.GetInt("Score") + 50;
            PlayerPrefs.SetInt("Score", score);
        }
    }
}
