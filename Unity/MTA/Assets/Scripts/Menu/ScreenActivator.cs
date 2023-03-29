using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenActivator : MonoBehaviour
{
    public GameObject screenUI;
    public bool startActive;
    public float screenTime;

    private bool opposite;

    private void Start()
    {
        opposite = !startActive;
        screenUI.SetActive(startActive);
        Invoke(nameof(ChangeScreen), screenTime);
    }

    private void ChangeScreen()
    {
        screenUI.SetActive(opposite);
    }
}
