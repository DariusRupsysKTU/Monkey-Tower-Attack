using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        // Checks if user clicked button 1
        if (Input.GetKeyDown(key1))
        {
            FadeToColor(item1.colors.pressedColor, item1);
            item1.onClick.Invoke();
        }
        else if (Input.GetKeyUp(key1))
        {
            FadeToColor(item1.colors.normalColor, item1);
        }

        // Checks if user clicked button 2
        if (Input.GetKeyDown(key2))
        {
            FadeToColor(item2.colors.pressedColor, item2);
            item2.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key2))
        {
            FadeToColor(item2.colors.normalColor, item2);
        }

        // Checks if user clicked button 3
        if (Input.GetKeyDown(key3))
        {
            FadeToColor(item3.colors.pressedColor, item3);
            item3.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key3))
        {
            FadeToColor(item3.colors.normalColor, item3);
        }

        // Checks if user clicked button 4
        if (Input.GetKeyDown(key4))
        {
            FadeToColor(item4.colors.pressedColor, item4);
            item4.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key4))
        {
            FadeToColor(item4.colors.normalColor, item4);
        }

        // Updates the player score
        score = PlayerPrefs.GetInt("Score");
        scoreUI.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
    }

    void FadeToColor(Color color, Button button)
    {
        Graphic graphic = button.GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }

    public void OnItemClicked()
    {
        
    }

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
        if(PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", PlayerPrefs.GetInt("Score"));
        }
        
        PlayerPrefs.SetInt("Score", 0);
        score = 0;
    }

    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public int currency = 0;
    public Text currencyUI = null;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        currencyUI.text = "Money: " + currency + "$";
    }

    public void LoadData(GameData data)
    {
        this.currency = data.currency;
        this.score = data.score;
        currencyUI.text = "Money: " + currency + "$";
        PlayerPrefs.SetInt("Score", score);
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
        data.score = score;
    }

    [Header("Score")]
    public int score = 0;
    public Text scoreUI = null;
    public Text highscoreUI = null;
}
