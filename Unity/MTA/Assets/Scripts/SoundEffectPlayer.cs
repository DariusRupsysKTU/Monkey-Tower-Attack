using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip buttonClick;
    public AudioClip monkeySelect;

    public void Button()
    {
        src.clip = buttonClick;
        src.Play();
    }

    public void MonkeySelect()
    {
        src.clip = monkeySelect;
        src.Play();
    }
}
