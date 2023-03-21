using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public double currency = 0;
    public Text currencyUI = null;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        currencyUI.text = "Money: " + currency + "$";
    }
}
