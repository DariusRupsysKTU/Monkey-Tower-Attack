using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveText : MonoBehaviour
{
    public string text;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip fadeInAnimation;
    [SerializeField] AnimationClip fadeOutAnimation;

    private void Start() 
    {
        GetComponent<TextMesh>().text = text;    
    }

    public void PlayFadeInAnimation() => animator.Play(fadeInAnimation.name, 0);

    public void PlayFadeOutAnimation() => animator.Play(fadeOutAnimation.name, 0);
}
