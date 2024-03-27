using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomLoader : MonoBehaviour
{
    private List<LevelSymptomButton> buttons;

    private void Awake()
    {
        buttons = new List<LevelSymptomButton>();
        foreach (Transform child in transform)
        {
            LevelSymptomButton button = child.GetComponent<LevelSymptomButton>();
            if (button != null)
            {
                buttons.Add(button);
            }
        }
    }

    public void LoadSymptoms()
    {
        foreach (LevelSymptomButton button in buttons)
        {
            if (button.isOn)
            {
                DataManager.instance.AddSymptom(button.symptom);
            }
        }
    }
}
