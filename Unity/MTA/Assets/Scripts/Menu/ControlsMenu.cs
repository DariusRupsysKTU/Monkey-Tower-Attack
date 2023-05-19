using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] GameObject moveControlText;
    [SerializeField] GameObject attack1ControlText;
    [SerializeField] GameObject attack2ControlText;
    [SerializeField] GameObject itemPickUpControlText;
    [SerializeField] GameObject enterShopControlText;
    [SerializeField] GameObject itemPrefab;

    private KeyCode attack1Key;
    private KeyCode attack2Key;
    private KeyCode itemPickUpKey;
    private KeyCode enterShopKey;

    void Update() 
    {
        if (attack1Key == KeyCode.None || itemPickUpKey == KeyCode.None || enterShopKey == KeyCode.None) 
        {
            GetButtons();
            ChangeText();
        }   
    }

    private void GetButtons()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<Shoot>() != null)
        {
            Shoot[] shootScripts = player.GetComponents<Shoot>();
            attack1Key = shootScripts[0].shootKey;
            if (shootScripts.Length == 2)
            {
                attack2Key = shootScripts[1].shootKey;
            }
        }
        else
        {
            attack1Key = player.GetComponent<Punch>().punchKey;
        }

        itemPickUpKey = itemPrefab.GetComponent<Interaction>().interactKey;

        enterShopKey = GameObject.Find("Shop").GetComponent<Interaction>().interactKey;
    }

    private void ChangeText()
    {
        moveControlText.GetComponent<TextMeshProUGUI>().text = string.Format("{0,-15} {1}", "move:", "wasd");
        attack1ControlText.GetComponent<TextMeshProUGUI>().text = string.Format("{0,-15} {1}", "attack 1:", attack1Key);
        attack2ControlText.GetComponent<TextMeshProUGUI>().text = string.Format("{0,-15} {1}", "attack 2:", attack2Key);
        itemPickUpControlText.GetComponent<TextMeshProUGUI>().text = string.Format("{0,-15} {1}", "item pick up:", itemPickUpKey);
        enterShopControlText.GetComponent<TextMeshProUGUI>().text = string.Format("{0,-15} {1}", "enter shop:", enterShopKey);
    }
}
