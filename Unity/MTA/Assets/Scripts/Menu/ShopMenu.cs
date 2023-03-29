using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public static bool ShopOpened = false;
    public GameObject player;
    //public GameObject shopMenuUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //shopMenuUI.SetActive(false);
    }


    void Update()
    {
        Vector2 pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().position;
        float distance = Vector2.Distance(transform.position, pos);
        if (Input.GetKeyDown(KeyCode.V) && distance < 0.35)
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
