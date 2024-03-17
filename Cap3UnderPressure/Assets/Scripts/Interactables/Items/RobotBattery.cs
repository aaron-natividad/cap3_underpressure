using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBattery : Item
{
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    public float chargePercentage;
    private Renderer rend;

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
        rend.material = IsFullyCharged() ? onMaterial : offMaterial;
    }

    public bool IsFullyCharged()
    {
        return chargePercentage >= 100f;
    }
}
