using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public static bool ShopOpened = false;
    //public GameObject shopMenuUI;

    void Start()
    {
        //shopMenuUI.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (ShopOpened)
            {
                //shopMenuUI.SetActive(false);
                Time.timeScale = 1f;
                ShopOpened = false;
            }
            else
            {
                //shopMenuUI.SetActive(true);
                Time.timeScale = 0f;
                ShopOpened = true;
            }
        }
    }
}
