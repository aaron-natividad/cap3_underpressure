using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionCounter : Machine
{
    public static Action<bool> OnRobotEvaluated;

    [SerializeField] private AudioClip conveyorSFX;
    [Space(10)]
    [SerializeField] private AudioClip correctSFX;
    [SerializeField] private AudioClip wrongSFX;

    [Header("Machine Components")]
    [SerializeField] private SubmissionLight subLight;
    [SerializeField] private Transform cylinderParent;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform destroyPoint;

    [Header("Parameters")]
    [SerializeField] private float rotationSpeed;

    private RobotColor requiredColor;

    protected override void AddListeners()
    {
        base.AddListeners();
        ScoreHandler.OnColorRequirementChanged += ReceiveColorRequirement;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ScoreHandler.OnColorRequirementChanged -= ReceiveColorRequirement;
    }

    private void Update()
    {
        foreach (Transform t in cylinderParent)
        {
            t.Rotate(new Vector3(rotationSpeed * Time.deltaTime, 0, 0), Space.Self);
        }
    }

    public override void Interact(Player pc)
    {
        if (state != MachineState.Normal) return;
        if (heldItem != null || pc.heldItem == null) return;
        TakeItem(pc, attachPoint);
        StartCoroutine(PerformAction());
        OnMachineInteracted?.Invoke(this);
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;
        audioSource.clip = conveyorSFX;
        audioSource.Play();
        LeanTween.move(heldItem.gameObject, destroyPoint.transform.position, 2f);
        yield return new WaitForSeconds(2f);

        audioSource.clip = null;
        audioSource.Stop();

        Robot robot = heldItem.GetComponent<Robot>();
        if (robot != null)
        {
            PlayEffect(robot.IsCorrect(requiredColor));
            OnRobotEvaluated?.Invoke(robot.IsCorrect(requiredColor));
        }
        else
        {
            PlayEffect(false);
        }

        ClearItem();
        state = MachineState.Normal;
    }

    private void PlayEffect(bool isCorrect)
    {
        audioSource.PlayOneShot(isCorrect ? correctSFX : wrongSFX);
        subLight.DisplayColor(isCorrect, 0.5f);
    }

    private void ReceiveColorRequirement(RobotColor color)
    {
        requiredColor = color;
    }
}
