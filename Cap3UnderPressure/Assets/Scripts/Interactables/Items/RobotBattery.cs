using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBattery : Item
{
    public Action OnFullCharge;

    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    public float chargePercentage;
    private Renderer rend;
    private bool isFull;

    protected override void Initialize()
    {
        base.Initialize();
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material);
        rend.material = IsFullyCharged() ? onMaterial : offMaterial;
    }

    public void Charge(float addPercentage)
    {
        chargePercentage += addPercentage;

        if (chargePercentage >= 100f && !isFull)
        {
            isFull = true;
            OnFullCharge?.Invoke();
        }

        rend.material = IsFullyCharged() ? onMaterial : offMaterial;
    }

    public bool IsFullyCharged()
    {
        return isFull;
    }
}
