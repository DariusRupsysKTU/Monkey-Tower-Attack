using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, DataPersistence
{
    public static Inventory instance { get; private set; }

    [Header("Inventory items")]
    [SerializeField] private Button item1;
    [SerializeField] private Button item2;
    [SerializeField] private Button item3;
    [SerializeField] private Button item4;

    public KeyCode key1;
    public KeyCode key2;
    public KeyCode key3;
    public KeyCode key4;

    //public Color pressedColor = new Color(0.6698113f, 0.647063f, 0.647063f, 1f);
    //public Color normalColor = new Color(1f, 1f, 1f, 1f);

    private void Start()
    {
        key1 = KeyCode.Alpha1;
        key2 = KeyCode.Alpha2;
        key3 = KeyCode.Alpha3;
        key4 = KeyCode.Alpha4;

        highscoreUI.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }

    void Update()
    {
        /*// Checks if user clicked button 1
        if (Input.GetKeyDown(key1))
        {
            FadeToColor(item1.colors.pressedColor, item1);
            item1.onClick.Invoke();
        }
        else if (Input.GetKeyUp(key1))
        {
            FadeToColor(item1.colors.normalColor, item1);
        }*/
    }

    /*void FadeToColor(Color color, Button button)
    {
        Graphic graphic = button.GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }*/

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1f;
    }

    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public int currency = 0;
    public int totalCurrency = 0;
    public Text currencyUI = null;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        totalCurrency += amount;
        currencyUI.text = "Money: " + currency + "$";

        PlayerPrefs.SetInt("Total money", totalCurrency);
    }
    public void DecreaseCurrency(int amount)
    {
        currency -= amount;
        currencyUI.text = "Money: " + currency + "$";
    }

    [Header("Score")]
    public int score = 0;
    public Text scoreUI = null;
    public Text highscoreUI = null;

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreUI.text = "Score: " + score.ToString();
    }

    public void LoadData(GameData data)
    {
        this.currency = data.currency;
        this.totalCurrency = data.totalCurrency;
        this.score = data.score;
        currencyUI.text = "Money: " + this.currency + "$";
        scoreUI.text = "Score: " + score.ToString();
    }

    public void SaveData(ref GameData data)
    {
        data.currency = this.currency;
        data.totalCurrency = this.totalCurrency;
        data.score = this.score;
    }
}
