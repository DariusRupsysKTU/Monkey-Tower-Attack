using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }

    [SerializeField] private Button item1;
    [SerializeField] private Button item2;
    [SerializeField] private Button item3;
    [SerializeField] private Button item4;

    public KeyCode key1;
    public KeyCode key2;
    public KeyCode key3;
    public KeyCode key4;

    private void Start()
    {
        key1 = KeyCode.Alpha1;
        key2 = KeyCode.Alpha2;
        key3 = KeyCode.Alpha3;
        key4 = KeyCode.Alpha4;
    }

    void Update()
    {
        // Checks if user clicked the button
        if (Input.GetKeyDown(key1))
        {
            FadeToColor(item1.colors.normalColor, item1);
            item1.onClick.Invoke();
            Debug.Log("Pressed 1");
        }
        else if (Input.GetKeyUp(key1))
        {
            FadeToColor(item1.colors.normalColor, item1);
        }

        if(Input.GetKeyDown(key2))
        {
            FadeToColor(item2.colors.pressedColor, item2);
            item2.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key2))
        {
            FadeToColor(item2.colors.normalColor, item2);
        }

        if(Input.GetKeyDown(key3))
        {
            FadeToColor(item3.colors.pressedColor, item3);
            item3.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key3))
        {
            FadeToColor(item3.colors.normalColor, item3);
        }

        if(Input.GetKeyDown(key4))
        {
            FadeToColor(item4.colors.pressedColor, item4);
            item4.onClick.Invoke();
        }
        else if(Input.GetKeyUp(key4))
        {
            FadeToColor(item4.colors.normalColor, item4);
        }

    }

    void FadeToColor(Color color, Button button)
    {
        Graphic graphic = GetComponent<Graphic>();
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

    public static bool inventoryActive = true;

    public GameObject inventoryUi;

    [Header("Currency")]
    public double currency = 0;
    public Text currencyUI = null;

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        currencyUI.text = "Money: " + currency + "$";
    }
}
