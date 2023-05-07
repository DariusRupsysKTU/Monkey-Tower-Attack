using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject optionMenuUI;
    public GameObject monkeySelectionMenuUI;

    public Button continueButton;

    public int selectedMonkey = 0;

    private void Start()
    {
        mainMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);
        monkeySelectionMenuUI.SetActive(false);

        Debug.Log(PlayerPrefs.GetInt("SaveDataExists"));
        Debug.Log(PlayerPrefs.GetInt("Total money"));

        if(PlayerPrefs.GetInt("SaveDataExists") == 1)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("SaveDataExists") == 1)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void ChangeMonkeySelection(int selection)
    {
        selectedMonkey = selection;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt(nameof(selectedMonkey), selectedMonkey);
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        SaveSystemManager.instance.NewGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        SaveSystemManager.instance.LoadGame();
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("NewGame", 0);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        SaveSystemManager.instance.SaveGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
