using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinManager : MonoBehaviour
{
    public int value = 100;
    private int score = 0;

    [SerializeField] private AudioSource coinPickUp;
    // public Inventory inventory;

    /*private void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<Inventory>();
    }*/

    public void UpdateCoins()
    {
        coinPickUp.Play();

        Inventory.instance.IncreaseCurrency(value);
        score = PlayerPrefs.GetInt("Score") + 50;
        PlayerPrefs.SetInt("Score", score);
    }
}
