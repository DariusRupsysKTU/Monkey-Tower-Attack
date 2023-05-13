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

    // Start is called before the first frame update
    void Start()
    {
        /*if(PlayerPrefs.GetInt("LoadedLevel") == 1)
        {
            PlayerPrefs.SetInt("NextLevel", 0);
        }*/

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

        currencyFinal.text = "Money earned: " + PlayerPrefs.GetInt("Total money").ToString() + "$";
        scoreFinal.text = "Score: " + Inventory.instance.score.ToString();

        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        levelReached.text = "LEVEL: " + enemyManagerScript.levelNr.ToString();

        if (Inventory.instance.score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Inventory.instance.score);
        }
    }

    public void PressedNextLevel()
    {        
        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        PlayerPrefs.SetInt("NextLevel", 1);
        PlayerPrefs.SetInt("NewGame", 0);
        PlayerPrefs.SetInt("Level", enemyManagerScript.levelNr);

        SceneManager.LoadScene(1);
    }

    public void PressedBackToMainMenu()
    {
        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        PlayerPrefs.SetInt("NextLevel", 0);
        //PlayerPrefs.SetInt("LoadedLevel", 0);

        PlayerPrefs.SetInt("Level", enemyManagerScript.levelNr);
        SceneManager.LoadScene(0);
    }
}
