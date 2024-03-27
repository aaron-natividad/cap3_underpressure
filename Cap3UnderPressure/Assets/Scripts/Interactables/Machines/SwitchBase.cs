using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBase : Machine
{
    [Header("Game Objects")]
    [SerializeField] protected GameObject switchHandle;
    [SerializeField] protected Machine machine;

    [Header("Parameters")]
    [SerializeField] protected Vector3 switchRotation;

    protected Vector3 originalSwitchRotation;

    protected override void Initialize()
    {
        base.Initialize();
        if (switchHandle != null) originalSwitchRotation = switchHandle.transform.localRotation.eulerAngles;
    }

    public override void Interact(Player pc)
    {
        if (machine.state == MachineState.Performing || state != MachineState.Normal) return;
        if (TryDisable()) return;
        StartCoroutine(DoSwitchAction());
        OnMachineInteracted?.Invoke(this);
    }

    protected virtual IEnumerator DoSwitchAction()
    {
        state = MachineState.Performing;
        if (switchHandle != null) LeanTween.rotateLocal(switchHandle, switchRotation, 0.05f);
        machine.StartCoroutine(machine.PerformAction());

        yield return new WaitForSeconds(0.1f);
        if (switchHandle != null) LeanTween.rotateLocal(switchHandle, originalSwitchRotation, 0.5f);
        state = storedState;
    }

    protected override void DisableConnectedMachines()
    {
        machine.StartCoroutine(machine.CO_DisableAnimation(false, false));
    }
}
