using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private AudioSource coinPickUp;

    public void UpdateCoins(int value, int score)
    {
        coinPickUp.Play();

        Inventory.instance.IncreaseCurrency(value);

        Inventory.instance.IncreaseScore(score);
    }
}
