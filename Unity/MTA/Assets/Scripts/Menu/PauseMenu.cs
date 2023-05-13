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

    public bool cheatsOn = false;

    private void Start()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!LoadingScreen.LoadingScreenOn && !Victory.VictoryScreenOn && 
        !GameOver.GameOverScreenOn && Input.GetKeyDown(KeyCode.Escape) && !ShopMenu.ShopOpened)
        {
            if (GameIsPaused)
            {
                Resume();
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

        // player damage ir movement speed rodyti

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

    public void TurnCheats()
    {
        bool opposite = !cheatsOn;
        cheatsOn = opposite;
    }
}
