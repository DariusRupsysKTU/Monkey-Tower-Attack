using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    
    public GameObject pauseMenuUI;
    public Text levelText;

    private void Start()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Victory.VictoryScreenOn && !GameOver.GameOverScreenOn && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else if (PlayerPrefs.GetInt("ShopOpen") == 1)
            {
                PlayerPrefs.SetInt("ShopOpen", 0);
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        EnemyManager enemyManagerScript = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        levelText.text = "LEVEL: " + enemyManagerScript.levelNr.ToString();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void PressedResume()
    {
        Resume();
    }

    public void PressedBackToMainMenu()
    {
        SceneManager.LoadScene(0);
        SaveSystemManager.instance.SaveGame();
    }
}
