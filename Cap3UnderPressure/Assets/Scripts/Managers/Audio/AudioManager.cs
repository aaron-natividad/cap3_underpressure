using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer mainMixer;
    public AudioSource sourceBGM;
    public AudioSource sourceAmbient;

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
        sourceBGM.Play();
        sourceAmbient.Play();
        StartCoroutine(FadeAudio(0, 1, 0.5f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAudio(1, 0, 0.5f));
    }

    private IEnumerator FadeAudio(float startVolume, float endVolume, float duration)
    {
        float t = 0;
        float currentVolume;

        while (t < duration)
        {
            t += Time.deltaTime;
            currentVolume = Mathf.Lerp(startVolume, endVolume, t / duration);
            sourceBGM.volume = currentVolume;
            sourceAmbient.volume = currentVolume;
            yield return null;
        }

        sourceBGM.volume = endVolume;
        sourceAmbient.volume = endVolume;

        if (endVolume <= 0)
        {
            sourceBGM.Stop();
            sourceAmbient.Stop();
        }
    }
}
