using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frustration", menuName = "Symptoms/Frustration", order = 2)]
public class Frustration : Symptom
{
    [Header("Frustration Parameters")]
    public float disableTime;
    public int disableRate;

    public override void StartBehavior(SymptomsHandler handler)
    {
        base.StartBehavior(handler);
        handler.ActivateDisableRate(disableRate, disableTime);
        handler.StartCoroutine(UpdateCounterValue(handler));
    }

    private IEnumerator UpdateCounterValue(SymptomsHandler handler)
    {
        while (true)
        {
            ui.UpdateCounter(handler.currentDisableRate);
            yield return null;
        }
    }
}
