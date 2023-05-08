using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOptions : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.name.Contains("3"))
        {
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(3).gameObject.SetActive(false);
        }
    }
}
