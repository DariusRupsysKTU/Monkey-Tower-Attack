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

        /*if(SaveSystemManager. == null)
        {
            continueButton.gameObject.SetActive(false);
        }*/
    }

    private void Awake()
    {
        
    }

    public void ChangeMonkeySelection(int selection)
    {
        selectedMonkey = selection;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt(nameof(selectedMonkey), selectedMonkey);
        SceneManager.LoadScene(1);
        SaveSystemManager.instance.NewGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        SaveSystemManager.instance.LoadGame();
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
