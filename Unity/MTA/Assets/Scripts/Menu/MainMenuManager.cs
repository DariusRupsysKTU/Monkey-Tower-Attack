using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject optionMenuUI;
    public GameObject monkeySelectionMenuUI;

    public int selectedMonkey = 0;

    private void Start()
    {
        mainMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);
        monkeySelectionMenuUI.SetActive(false);
    }

    public void ChangeMonkeySelection(int selection)
    {
        selectedMonkey = selection;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt(nameof(selectedMonkey), selectedMonkey);
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
