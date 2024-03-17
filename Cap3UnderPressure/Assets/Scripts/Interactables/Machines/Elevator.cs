using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Elevator : Machine
{
    [SerializeField] private GameObject elevatorParent;
    [SerializeField] private Transform door;
    [SerializeField] private SceneHandler sceneHandler;
    [Space(10)]
    [SerializeField] private float liftDistance;
    [SerializeField] private float liftDuration;
    [SerializeField] private float doorDistance;
    [SerializeField] private float doorDuration;


    public override void Interact(Player player)
    {
        
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;
        LeanTween.moveLocal(door.gameObject, door.localPosition + Vector3.down * doorDistance, doorDuration).setEase(LeanTweenType.easeInCubic);
        yield return new WaitForSeconds(doorDuration + 1f);

        LeanTween.move(elevatorParent, elevatorParent.transform.position + Vector3.up * liftDistance, liftDuration);
        yield return new WaitForSeconds(2f);

        sceneHandler.LoadNextScene();
        state = MachineState.Normal;
    }
}
