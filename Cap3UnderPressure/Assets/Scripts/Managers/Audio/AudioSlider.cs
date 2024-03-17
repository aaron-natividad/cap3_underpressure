using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string volumeName;
    
    private void Start()
    {
        float volumeValue = PlayerPrefs.HasKey(volumeName) ? PlayerPrefs.GetFloat(volumeName) : 0;
        volumeSlider.value = volumeValue;
    }

    public void ChangeVolume()
    {
        AudioManager.instance?.ChangeVolume(volumeName, volumeSlider.value);
    }

    public void ResetVolume()
    {
        AudioManager.instance?.ChangeVolume(volumeName, 0);
        volumeSlider.value = 0;
    }
}
