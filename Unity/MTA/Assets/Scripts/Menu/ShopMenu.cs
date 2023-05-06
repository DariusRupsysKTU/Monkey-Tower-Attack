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
        player = GameObject.FindGameObjectWithTag("Player");
        shopMenuUI.SetActive(false);
    }


    void Update()
    {

        //if (player != null)
        //{
        //    aboveText.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
        //}

        Vector2 pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().position;
        float distance = Vector2.Distance(transform.position, pos);
        if (Input.GetKeyDown(KeyCode.V) && distance < 0.35)
        {
            if (ShopOpened)
            {
                shopMenuUI.SetActive(false);
                Time.timeScale = 1f;
                ShopOpened = false;
            }
            else
            {
                shopMenuUI.SetActive(true);
                Time.timeScale = 0f;
                ShopOpened = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ShopOpened)
            {
                shopMenuUI.SetActive(false);
                Time.timeScale = 1f;
                ShopOpened = false;
            }
        }
    }
    public void BuyHeal()
    {
        if(Inventory.instance.currency >= 50 && PlayerHealth.instance.playerHealth < 5)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().HealPlayer(1);
            Inventory.instance.DecreaseCurrency(50);
        }
    }
    public void DMG()
    {
        if (Inventory.instance.currency >= 100)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>().enabled)
            {
                PlayerBullet bulletScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>();
                bulletScript.EnableDoubleDamage(10f);
                StartCoroutine(DisableDamage(bulletScript, 10f));
                Inventory.instance.DecreaseCurrency(100);
            }
        }
    }
    public void Speed()
    {
        if (Inventory.instance.currency >= 100)
        {
            float speedBoostDuration = 5f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnableSpeedBoost(speedBoostDuration);
            Inventory.instance.DecreaseCurrency(100);
        }
    }
    IEnumerator DisableDamage(PlayerBullet bulletScript, float time)
    {
        yield return new WaitForSeconds(time);
        bulletScript.DisableExtraDamage();
    }
}
