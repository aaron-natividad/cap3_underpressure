using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance;

    public AudioClip bgmClip;
    public AudioClip ambientClip;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        AudioManager.instance.sourceBGM.clip = bgmClip;
        AudioManager.instance.sourceAmbient.clip = ambientClip;
        AudioManager.instance.FadeIn();
    }
}
