using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSymptomButton : MonoBehaviour
{
    public Symptom symptom;

    [HideInInspector] public bool isOn;
    private Toggle toggle;
    private Image image;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();

        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(false);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        image.color = isOn ? new Color(1, 1, 1) : new Color(0.5f, 0.5f, 0.5f);
        this.isOn = isOn;
    }
}
