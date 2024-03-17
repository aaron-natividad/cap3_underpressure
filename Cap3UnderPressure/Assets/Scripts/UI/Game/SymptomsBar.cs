using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SymptomsBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI symptomsTitle;
    [SerializeField] private SymptomIcon[] symptomIcons;

    public void InitializeUI(List<Symptom> symptomData)
    {
        if (symptomData.Count <= 0)
        {
            DisableBar();
            return;
        }

        for (int i = 0; i < symptomData.Count; i++)
        {
            symptomData[i].ui = symptomIcons[i];
        }
    }

    public void DisableBar()
    {
        symptomsTitle.enabled = false;
        foreach (SymptomIcon si in symptomIcons)
        {
            si.DisableIcon();
        }
    }
}
