using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static bool GameOverScreenOn;

    public GameObject gameOverMenu;
    public GameObject inventory;

    private PlayerHealth playerHealthScript;

    public Text currencyFinal = null;
    public Text scoreFinal = null;
    public Text levelReached = null;

    public Text damageText;
    public Text speedText;
    public Text cooldownText;

    // Start is called before the first frame update
    void Start()
    {
        GameOverScreenOn = false;
        PlayerPrefs.SetInt("Dead", 0);
        gameOverMenu.SetActive(false);
        inventory.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealthScript == null)
        {
            playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }

        if(playerHealthScript.playerHealth <= 0)
        {
            GameOverEnable();
            inventory.SetActive(false);
        }
    }

    void GameOverEnable()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
        GameOverScreenOn = true;

        // gets player stats
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.transform.name.Contains("1"))
        {
            damageText.text = "Damage: " + player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>().damage.ToString();
            speedText.text = "Speed: " + player.GetComponent<PlayerMovement>().moveSpeed.ToString();
            cooldownText.text = "";
        }
        else if (player.transform.name.Contains("2"))
        {
            damageText.text = "Damage: " + player.GetComponent<Punch>().damage.ToString();
            speedText.text = "Speed: " + player.GetComponent<PlayerMovement>().moveSpeed.ToString();
            cooldownText.text = "";
        }
        else
        {
            if (player.GetComponent<Shoot>().bulletPrefab.name.Contains("Fireball"))
            {
                damageText.text = "Damage: " + player.GetComponent<Shoot>().bulletPrefab.GetComponent<PlayerBullet>().damage.ToString();
            }

            speedText.text = "Speed: " + player.GetComponent<PlayerMovement>().moveSpeed.ToString();

            cooldownText.text = "Cooldown: " + player.GetComponent<Shoot>().shootCooldown.ToString();
        }

        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        levelReached.text = "LEVEL: " + enemyManagerScript.levelNr.ToString();

        currencyFinal.text = "Money earned: " + PlayerPrefs.GetInt("Total money").ToString() + "$";
        scoreFinal.text = "Score: " + Inventory.instance.score.ToString(); 

        PlayerPrefs.SetInt("SaveDataExists", 0);

        if (Inventory.instance.score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Inventory.instance.score);
        }

        PlayerPrefs.SetInt("Total money", 0);
    }

    public void PressedBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
