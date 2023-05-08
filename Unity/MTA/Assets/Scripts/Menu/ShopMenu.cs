using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public static bool ShopOpened = false;
    public GameObject player;
    public GameObject shopMenuUI;
    //private GameObject aboveText;

    void Start()
    {
        shopMenuUI.SetActive(false);
        PlayerPrefs.SetInt("ShopOpen", 0);
    }
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Interaction interactionScript = GetComponent<Interaction>();

            if (Input.GetKeyDown(interactionScript.interactKey) && interactionScript.inRange)
            {
                if (ShopOpened)
                {
                    Time.timeScale = 1f;
                    shopMenuUI.SetActive(false);
                    ShopOpened = false;
                    PlayerPrefs.SetInt("ShopOpen", 0);
                }
                else
                {
                    shopMenuUI.SetActive(true);
                    Time.timeScale = 0f;
                    ShopOpened = true;
                    PlayerPrefs.SetInt("ShopOpen", 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ShopOpened)
                {
                    Time.timeScale = 1f;
                    shopMenuUI.SetActive(false);
                    ShopOpened = false;
                }
            }
        }
        
    }

    public void BuyHeal()
    {
        if(Inventory.instance.currency >= 50 && PlayerHealth.instance.playerHealth < 5)
        {
            player.GetComponent<PlayerHealth>().HealPlayer(1);
            Inventory.instance.DecreaseCurrency(50);
        }
    }

    public void BuyDMG()
    {
        if (Inventory.instance.currency >= 100)
        {
            if (player.GetComponent<Shoot>().enabled)
            {
                PlayerBullet bulletScript = player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>();
                bulletScript.IncreaseDamage();
                Inventory.instance.DecreaseCurrency(100);
            }
            else if (player.GetComponent<Punch>().enabled)
            {
                player.GetComponent<Punch>().IncreaseDamage();
                Inventory.instance.DecreaseCurrency(100);
            }
        }
    }
    public void BuySpeed()
    {
        if (Inventory.instance.currency >= 100)
        {
            player.GetComponent<PlayerMovement>().IncreaseSpeed();
            Inventory.instance.DecreaseCurrency(100);
        }
    }
}
