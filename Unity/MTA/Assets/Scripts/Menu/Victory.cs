using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public static bool VictoryScreenOn;

    public GameObject victoryMenu;
    public GameObject inventory;

    public bool victory = false;

    public Text currencyFinal = null;
    public Text scoreFinal = null;
    public Text levelReached = null;

    public Text damageText;
    public Text speedText;
    public Text cooldownText;

    // Start is called before the first frame update
    void Start()
    {
        VictoryScreenOn = false;
        victoryMenu.SetActive(false);
        inventory.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (victory)
        {
            VictoryEnable();
            inventory.SetActive(false);
        }
    }

    void VictoryEnable()
    {
        //Time.timeScale = 0f;
        victoryMenu.SetActive(true);
        VictoryScreenOn = true;

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

        currencyFinal.text = "Money earned: " + PlayerPrefs.GetInt("Total money").ToString() + "$";
        scoreFinal.text = "Score: " + Inventory.instance.score.ToString();

        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        levelReached.text = "LEVEL: " + enemyManagerScript.levelNr.ToString();

        if (Inventory.instance.score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Inventory.instance.score);
        }

        enemyManagerScript.increaseLevel();
        victory = false;
    }

    public void PressedNextLevel()
    {        
        PlayerPrefs.SetInt("NewGame", 0);

        SceneManager.LoadScene(1);
        SaveSystemManager.instance.SaveGame();
    }

    public void PressedBackToMainMenu()
    {
        SceneManager.LoadScene(0);
        SaveSystemManager.instance.SaveGame();
    }
}
