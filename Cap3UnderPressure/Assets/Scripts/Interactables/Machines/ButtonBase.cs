using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : Machine
{
    [SerializeField] private AudioClip buttonSound;

    [Header("Game Objects")]
    [SerializeField] protected GameObject buttonObject;
    [SerializeField] protected Machine machine;

    [Header("Parameters")]
    [SerializeField] protected Vector3 buttonLocation;

    protected Vector3 originalButtonLocation;

    protected override void Initialize()
    {
        base.Initialize();

        if (buttonObject != null)
            originalButtonLocation = buttonObject.transform.localPosition;
    }

    public override void Interact(Player pc)
    {
        if (machine.state == MachineState.Performing || state != MachineState.Normal) return;
        if (TryDisable()) return;
        StartCoroutine(DoButtonAction());
        OnMachineInteracted?.Invoke(this);
    }

    protected IEnumerator DoButtonAction()
    {
        state = MachineState.Performing;
        audioSource.PlayOneShot(buttonSound);
        if (buttonObject != null) LeanTween.moveLocal(buttonObject, buttonLocation, 0.05f);
        machine.StartCoroutine(machine.PerformAction());

        yield return new WaitForSeconds(0.1f);
        if (buttonObject != null) LeanTween.moveLocal(buttonObject, originalButtonLocation, 0.5f);
        state = storedState;
    }

    protected override void DisableConnectedMachines()
    {
        machine.StartCoroutine(machine.CO_DisableAnimation(false, false));
    }
}
