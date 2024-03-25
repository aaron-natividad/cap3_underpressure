using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer mainMixer;

    [Header("BGM")]
    public AudioSource sourceBGM_Base;
    public AudioSource sourceBGM_Ambient;
    public AudioSource sourceBGM_TrackOne;
    public AudioSource sourceBGM_TrackTwo;

    [Header("Ambient")]
    public AudioSource sourceAmbient;

    private bool isTrackOne;
    public AudioSource queuedTrack;

    private void OnEnable()
    {
        SceneHandler.OnSceneLoading += FadeOut;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneLoading -= FadeOut;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        isTrackOne = true;
        queuedTrack = sourceBGM_TrackTwo;
        sourceBGM_TrackTwo.mute = true;
    }

    private void Start()
    {
        GetMixerValue("Master");
        GetMixerValue("BGM");
        GetMixerValue("SFX");
    }

    public void ChangeVolume(string volumeName, float value)
    {
        PlayerPrefs.SetFloat(volumeName, value);
        mainMixer.SetFloat(volumeName, value);
    }

    private void GetMixerValue(string volumeName)
    {
        float volumeValue = PlayerPrefs.HasKey(volumeName) ? PlayerPrefs.GetFloat(volumeName) : 0;
        ChangeVolume(volumeName, volumeValue);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeAudio(0, 1, 1f));
        Play();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAudio(1, 0, 1f));
    }

    public void Play()
    {
        sourceBGM_Ambient.Play();
        sourceBGM_Base.Play();
        sourceBGM_TrackOne.Play();
        sourceBGM_TrackTwo.Play();
        sourceAmbient.Play();
    }

    public void Pause()
    {
        sourceBGM_Ambient.Pause();
        sourceBGM_Base.Pause();
        sourceBGM_TrackOne.Pause();
        sourceBGM_TrackTwo.Pause();
        sourceAmbient.Pause();
    }

    public void Stop()
    {
        sourceBGM_Ambient.Stop();
        sourceBGM_Base.Stop();
        sourceBGM_TrackOne.Stop();
        sourceBGM_TrackTwo.Stop();
        sourceAmbient.Stop();
    }

    public void SwitchTrack()
    {
        isTrackOne = !isTrackOne;
        queuedTrack = isTrackOne ? sourceBGM_TrackTwo : sourceBGM_TrackOne;
        sourceBGM_TrackOne.mute = !isTrackOne;
        sourceBGM_TrackTwo.mute = isTrackOne;
    }

    public void ClearTracks()
    {
        sourceBGM_TrackOne.clip = null;
        sourceBGM_TrackTwo.clip = null;
    }

    private void ChangeAllVolumes(float volume)
    {
        sourceBGM_Ambient.volume = volume;
        sourceBGM_Base.volume = volume;
        sourceBGM_TrackOne.volume = volume;
        sourceBGM_TrackTwo.volume = volume;
        sourceAmbient.volume = volume;
    }

    private IEnumerator FadeAudio(float startVolume, float endVolume, float duration)
    {
        float t = 0;
        float currentVolume;

        while (t < duration)
        {
            t += Time.deltaTime;
            currentVolume = Mathf.Lerp(startVolume, endVolume, t / duration);
            ChangeAllVolumes(currentVolume);
            yield return null;
        }

        ChangeAllVolumes(endVolume);
        if (endVolume <= 0)
        {
            Stop();
            ClearTracks();
        }
    }
}
