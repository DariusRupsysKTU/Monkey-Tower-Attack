using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public static bool ShopOpened;
    public GameObject player;
    [SerializeField] GameObject shopMenuUI;

    void Start()
    {
        ShopOpened = false;
        shopMenuUI.SetActive(false);
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

            if (!LoadingScreen.LoadingScreenOn && !PauseMenu.GameIsPaused && 
            Input.GetKeyDown(interactionScript.interactKey) && interactionScript.inRange)
            {
                if (ShopOpened)
                {
                    CloseShopWindow();
                }
                else
                {
                    OpenShopWindow();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ShopOpened)
                {
                    CloseShopWindow();
                    
                }
            }
        }
        
    }

    private void OpenShopWindow()
    {
        Time.timeScale = 0f;
        shopMenuUI.SetActive(true);
        ShopOpened = true;
    }

    private void CloseShopWindow()
    {
        Time.timeScale = 1f;
        shopMenuUI.SetActive(false);
        Invoke(nameof(CloseShop), 0.2f);
    }

    private void CloseShop()
    {
        ShopOpened = false;
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
            if (player.GetComponent<Shoot>() != null)
            {
                if (player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>() != null)
                {
                    PlayerBullet bulletScript = player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>();
                    bulletScript.IncreaseDamage();
                }
                Inventory.instance.DecreaseCurrency(100);
            }
            else if (player.GetComponent<Punch>() != null)
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
            player.GetComponent<PlayerMovement>().IncreaseSpeed(0.1f);
            Inventory.instance.DecreaseCurrency(100);
        }
    }
    public void BuyCooldown()
    {
        if (Inventory.instance.currency >= 75)
        {
            if (player.GetComponent<Shoot>().enabled)
            {
                player.GetComponent<Shoot>().DecreaseCooldown(0.2f);
                Inventory.instance.DecreaseCurrency(75);
            }
        }
    }
}
