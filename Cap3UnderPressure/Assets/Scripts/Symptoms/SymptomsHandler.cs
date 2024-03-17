using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymptomsHandler : MonoBehaviour
{
    public static SymptomsHandler instance;

    public SymptomsBar symptomsBar;
    public Image eyelids;
    public RecoveryStationGroup recoveryStationGroup;

    [HideInInspector] public float disableTime;
    private int disableRate;
    public int currentDisableRate;
    private bool canDisable;

    private List<Symptom> symptomData = new List<Symptom>();
    

    private void OnEnable()
    {
        Manager.OnStartGame += StartSymptoms;
        Manager.OnEndGame += EndSymptoms;
    }

    private void OnDisable()
    {
        Manager.OnStartGame -= StartSymptoms;
        Manager.OnEndGame -= EndSymptoms;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        canDisable = false;
    }

    private void StartSymptoms()
    {
        symptomData = DataManager.instance.currentSymptoms;
        symptomsBar.InitializeUI(symptomData);

        foreach (Symptom s in symptomData)
        {
            s.StartBehavior(this);
        }
    }

    private void EndSymptoms()
    {
        StopAllCoroutines();
        foreach (Symptom s in symptomData)
            s.EndBehavior(this);
    }

    public void ActivateDisableRate(int rate, float time)
    {
        disableRate = rate;
        disableTime = time;
        currentDisableRate = disableRate;
        canDisable = true;
    }

    public bool CheckDisableRate()
    {
        if (!canDisable) return false;

        currentDisableRate--;
        if (currentDisableRate <= 0)
        {
            currentDisableRate = disableRate;
            return true;
        }

        return false;
    }
}
