using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Machine
{
    public static Action OnElevatorLift;

    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip liftSound;
    [Space(10)]
    [SerializeField] private GameObject elevatorParent;
    [SerializeField] private Collider doorHitbox;
    [SerializeField] private Transform door;
    [SerializeField] private SceneHandler sceneHandler;
    [Space(10)]
    [SerializeField] private float liftDistance;
    [SerializeField] private float liftDuration;
    [SerializeField] private float doorDistance;
    [SerializeField] private float doorDuration;

    protected override void Initialize()
    {
        base.Initialize();
        doorHitbox.enabled = false;
    }

    public override void Interact(Player player)
    {
        
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;
        audioSource.PlayOneShot(doorSound);
        doorHitbox.enabled = true;
        LeanTween.moveLocal(door.gameObject, door.localPosition + Vector3.down * doorDistance, doorDuration).setEase(LeanTweenType.easeInCubic);
        yield return new WaitForSeconds(doorDuration + 1f);

        audioSource.PlayOneShot(liftSound);
        LeanTween.move(elevatorParent, elevatorParent.transform.position + Vector3.up * liftDistance, liftDuration);
        yield return new WaitForSeconds(2f);

        OnElevatorLift?.Invoke();
        state = storedState;
    }
}
