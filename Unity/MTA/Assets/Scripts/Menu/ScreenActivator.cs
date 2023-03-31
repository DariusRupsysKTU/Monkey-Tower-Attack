using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenActivator : MonoBehaviour
{
    public GameObject screenUI;
    public bool startActive;
    public float screenTime;
    public TransitionPlayer transitionScript;

    private bool opposite;

    private void Start()
    {
        opposite = !startActive;
        screenUI.SetActive(startActive);
        StartCoroutine(nameof(ChangeScreen));
        // Invoke(nameof(ChangeScreen), screenTime);
    }

    IEnumerator ChangeScreen()
    {
        yield return new WaitForSeconds(screenTime);
        if (this.transform.name == "LoadingCanvas")
        {
            transitionScript.PlayLoadingFadeOut();
        }
        yield return new WaitForSeconds(screenTime);
        screenUI.SetActive(opposite);
    }
}
