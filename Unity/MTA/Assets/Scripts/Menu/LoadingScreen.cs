using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public static bool LoadingScreenOn;

    void Start()
    {
        LoadingScreenOn = true;
    }

    private void OnDisable() 
    {
        LoadingScreenOn = false;
    }
}
