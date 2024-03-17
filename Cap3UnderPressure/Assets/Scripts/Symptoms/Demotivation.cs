using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Demotivation", menuName = "Symptoms/Demotivation", order = 3)]
public class Demotivation : Symptom
{
    public static Action<float> OnAnimationSpeedChange;

    [SerializeField] private float animSpeed;

    public override void StartBehavior(SymptomsHandler handler)
    {
        base.StartBehavior(handler);
        OnAnimationSpeedChange?.Invoke(animSpeed);
    }
}
