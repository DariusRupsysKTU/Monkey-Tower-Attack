using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveTextAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip fadeInAnimation;
    [SerializeField] AnimationClip fadeOutAnimation;

    public void PlayFadeInAnimation() => animator.Play(fadeInAnimation.name, 0);

    public void PlayFadeOutAnimation() => animator.Play(fadeOutAnimation.name, 0);
}
