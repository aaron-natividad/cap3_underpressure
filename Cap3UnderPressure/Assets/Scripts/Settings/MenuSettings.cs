using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private AudioSlider[] audioSliders;
    [SerializeField] private SensitivitySlider[] sensitivitySliders;

    public void ResetValues()
    {
        foreach (AudioSlider a in audioSliders)
        {
            a.ResetValue();
        }

        foreach (SensitivitySlider s in sensitivitySliders)
        {
            s.ResetValue();
        }
    }
}
