using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource audioSource;

    public AudioClip tileClickSound;

    public void PlayOneShout(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
