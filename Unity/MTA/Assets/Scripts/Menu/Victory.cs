using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject victoryMenu;
    public GameObject inventory;

    public Text currencyFinal = null;
    public Text scoreFinal = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Victory", 0);
        victoryMenu.SetActive(false);
        inventory.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Victory") == 1)
        {
            VictoryEnable();
            inventory.SetActive(false);
        }
    }

    void VictoryEnable()
    {
        victoryMenu.SetActive(true);
        Time.timeScale = 0f;

        currencyFinal.text = "Money earned: " + PlayerPrefs.GetInt("Total money").ToString() + "$";
        scoreFinal.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();

        PlayerPrefs.SetInt("SaveDataExists", 0);
        PlayerPrefs.SetInt("Victory", 0);

        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", PlayerPrefs.GetInt("Score"));
        }

        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Total money", 0);
    }

    public void PressedBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
