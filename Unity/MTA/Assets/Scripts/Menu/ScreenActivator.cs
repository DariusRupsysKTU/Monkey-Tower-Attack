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
    }

    IEnumerator ChangeScreen()
    {
        if (this.transform.name == "LoadingCanvas")
        {
            yield return new WaitForSeconds(screenTime);
            transitionScript.PlayLoadingFadeOut();
        }
        yield return new WaitForSeconds(screenTime);
        screenUI.SetActive(opposite);
    }
}
