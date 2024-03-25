using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance;

    [Header("BGM")]
    public AudioClip bgmAmbient;
    public AudioClip bgmBase;
    public AudioClip[] bgmBeats;

    [Header("Ambient")]
    public AudioClip ambient;

    private int beatIndex = 0;

    private void OnEnable()
    {
        SceneHandler.OnSceneReady += StartAudio;
        Manager.OnStartGame += QueueBeat;
        Timer.OnTimerHalfway += QueueBeat;
        Timer.OnTimerCritical += QueueBeat;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneReady -= StartAudio;
        Manager.OnStartGame += QueueBeat;
        Timer.OnTimerHalfway -= QueueBeat;
        Timer.OnTimerCritical -= QueueBeat;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        beatIndex = 0;
    }

    private void StartAudio()
    {
        AudioManager.instance.sourceBGM_Ambient.clip = bgmAmbient;
        AudioManager.instance.sourceBGM_Base.clip = bgmBase;
        AudioManager.instance.sourceAmbient.clip = ambient;

        if (bgmBeats.Length > 0)
        {
            StartCoroutine(BeatTest());
        }
        
        AudioManager.instance.FadeIn();
    }

    private IEnumerator BeatTest()
    {
        int index = 0;
        AudioManager.instance.sourceBGM_TrackOne.clip = bgmBeats[0];
        if (beatIndex < bgmBeats.Length - 1)
            AudioManager.instance.queuedTrack.clip = bgmBeats[beatIndex + 1];

        while (true)
        {
            yield return new WaitForSeconds(4f);
            if (beatIndex != index)
            {
                AudioManager.instance.SwitchTrack();
                if (beatIndex < bgmBeats.Length - 1)
                {
                    AudioManager.instance.queuedTrack.clip = bgmBeats[beatIndex + 1];
                    AudioManager.instance.queuedTrack.Play();
                }
                index = beatIndex;
            }
        }
    }

    private void QueueBeat()
    {
        if (beatIndex < bgmBeats.Length - 1)
        {
            Debug.Log("Beat Queued");
            beatIndex++;
        }
    }
}
