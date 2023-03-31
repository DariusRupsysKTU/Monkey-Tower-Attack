using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPlayer : MonoBehaviour
{
    public Animator animator;

    public void PlayLoadingFadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void PlayCrossfadeTransitionStart()
    {
        animator.SetTrigger("Start");
    }

    public void PlayCrossfadeTransitionEnd()
    {
        animator.SetTrigger("End");
    }
}
