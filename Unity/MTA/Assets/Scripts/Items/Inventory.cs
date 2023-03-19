using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public int currency = 0;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }
}
