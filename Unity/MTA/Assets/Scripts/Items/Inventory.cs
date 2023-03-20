using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public int currency = 0;
    public Text currencyUI;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        currencyUI.text = "Money:" + currency + "$";
    }
}
