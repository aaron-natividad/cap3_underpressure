using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusclePain", menuName = "Symptoms/MusclePain", order = 3)]
public class MusclePain : Symptom
{
    public override void StartBehavior(SymptomsHandler handler)
    {
        base.StartBehavior(handler);
        Player.instance.heldItemRunEnabled = false;
    }
}
