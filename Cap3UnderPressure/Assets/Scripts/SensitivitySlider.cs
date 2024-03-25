using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public static Action OnSensitivityChanged;

    [SerializeField] private Slider slider;
    [SerializeField] private string keyName;

    private void Start()
    {
        float value = PlayerPrefs.HasKey(keyName) ? PlayerPrefs.GetFloat(keyName) : 2f;
        PlayerPrefs.SetFloat(keyName, value);
        slider.value = value;
        OnSensitivityChanged?.Invoke();
    }

    public void ChangeValue()
    {
        PlayerPrefs.SetFloat(keyName, slider.value);
        OnSensitivityChanged?.Invoke();
    }

    public void ResetVolume()
    {
        PlayerPrefs.SetFloat(keyName, 2f);
        OnSensitivityChanged?.Invoke();
        slider.value = 2f;
    }
}
