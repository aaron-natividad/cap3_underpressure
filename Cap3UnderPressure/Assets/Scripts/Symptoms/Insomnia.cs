using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Insomnia", menuName = "Symptoms/Insomnia", order = 1)]
public class Insomnia : Symptom
{
    [Header("Insomnia Parameters")]
    public int insomniaDelay;
    public float blinkTime;

    private Material eyelidsMaterial;
    private RecoveryStationGroup group;

    public override void StartBehavior(SymptomsHandler handler)
    {
        base.StartBehavior(handler);
        eyelidsMaterial = handler.eyelids.material;
        group = handler.recoveryStationGroup;
        eyelidsMaterial.SetFloat("_EyelidValue", 1.1f);

        handler.StartCoroutine(DoInsomnia(handler));
    }

    public override void EndBehavior(SymptomsHandler handler)
    {
        base.EndBehavior(handler);
        eyelidsMaterial.SetFloat("_EyelidValue", 1.1f);
    }

    private IEnumerator DoInsomnia(SymptomsHandler handler)
    {
        int seconds = insomniaDelay;
        while (true)
        {
            ui.UpdateCounter(seconds);
            yield return new WaitForSeconds(1f);
            seconds--;
            if(seconds <= 0)
            {
                seconds = insomniaDelay;
                handler.StartCoroutine(Blink(handler));
            }
        }
    }

    private IEnumerator Blink(SymptomsHandler handler)
    {
        handler.StartCoroutine(ChangeEyelidValue(-0.1f, blinkTime / 2));
        yield return new WaitForSeconds(blinkTime / 2);

        group.TakePlayerItem();
        handler.StartCoroutine(ChangeEyelidValue(1.1f, blinkTime / 2));
    }

    private IEnumerator ChangeEyelidValue(float toValue, float time)
    {
        float t = 0;
        float startValue = eyelidsMaterial.GetFloat("_EyelidValue");

        while (t < time)
        {
            t += Time.deltaTime;
            float eyelidValue = Mathf.Lerp(startValue, toValue, t / time);
            eyelidsMaterial.SetFloat("_EyelidValue", eyelidValue);
            yield return null;
        }

        eyelidsMaterial.SetFloat("_EyelidValue", toValue);
    }
}
