using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip buttonClick;

    public void Button()
    {
        src.clip = buttonClick;
        src.Play();
    }
}
