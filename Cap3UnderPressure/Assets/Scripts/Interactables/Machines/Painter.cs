using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Painter : Machine
{
    [SerializeField] private AudioClip robotArmSound;
    [SerializeField] private AudioClip spraySound;
    [Space(10)]
    [SerializeField] private PainterSwitch[] painterButtons;

    [Header("Painter Objects")]
    [SerializeField] private ParticleSystem paintSpray;
    [SerializeField] private GameObject headH;
    [SerializeField] private GameObject headV;
    [SerializeField] private GameObject movableCylinder;
    [SerializeField] private Transform attachPoint;

    [Header("Painter Parameters")]
    [SerializeField] private float headHDistance;
    [SerializeField] private float headVDistance;
    [SerializeField] private float movableCylinderDistance;
    [SerializeField] private float headSpeed;
    [SerializeField] private int zigZagAmount;

    [HideInInspector] public RobotColor currentPaintColor;

    private ParticleSystem.MainModule psmain;

    private void Update()
    {
        paintSpray.transform.position = headH.transform.position - new Vector3(0, 0.74f, 0);
    }

    protected override void Initialize()
    {
        base.Initialize();
        psmain = paintSpray.main;
    }

    public override void Interact(Player pc)
    {
        if (state != MachineState.Normal) return;

        if (pc.heldItem != null)
        {
            if (!pc.heldItem.GetComponent<RobotShell>()) return;
            TakeItem(pc, attachPoint);
            OnMachineInteracted?.Invoke(this);
        }
        else if (heldItem != null)
        {
            GiveItem(pc, ref heldItem);
        }
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;
        audioSource.PlayOneShot(robotArmSound);
        LeanTween.moveLocal(headH, headH.transform.localPosition - new Vector3(-headHDistance, 0, 0), 0.5f * animSpeed);
        LeanTween.moveLocal(headV, headV.transform.localPosition - new Vector3(0, 0,headVDistance), 0.5f * animSpeed);
        yield return new WaitForSeconds(0.5f * animSpeed);

        psmain.startColor = Constants.robotColors[(int)currentPaintColor];
        audioSource.clip = spraySound;
        audioSource.loop = true;
        audioSource.Play();
        paintSpray.Play();
        LeanTween.moveLocal(headV, headV.transform.localPosition - new Vector3(0, 0, -headVDistance * 2), headSpeed * animSpeed * 2 * zigZagAmount);
        LeanTween.moveLocal(movableCylinder, movableCylinder.transform.localPosition + new Vector3(0, movableCylinderDistance, 0), headSpeed * animSpeed * 2 * zigZagAmount);
        for (int i = 0; i < zigZagAmount; i++)
        {
            LeanTween.moveLocal(headH, headH.transform.localPosition - new Vector3(headHDistance * 2, 0, 0), headSpeed * animSpeed);
            yield return new WaitForSeconds(headSpeed * animSpeed);
            LeanTween.moveLocal(headH, headH.transform.localPosition - new Vector3(-headHDistance * 2, 0, 0), headSpeed * animSpeed);
            yield return new WaitForSeconds(headSpeed * animSpeed);
        }

        paintSpray.Stop();
        audioSource.loop = false;
        audioSource.Stop();
        ChangePartColor();
        audioSource.PlayOneShot(robotArmSound);
        LeanTween.moveLocal(headV, headV.transform.localPosition - new Vector3(0, 0, headVDistance), 0.5f * animSpeed);
        LeanTween.moveLocal(headH, headH.transform.localPosition - new Vector3(headHDistance, 0 ,0), 0.5f * animSpeed);
        LeanTween.moveLocal(movableCylinder, movableCylinder.transform.localPosition - new Vector3(0, movableCylinderDistance, 0), 0.5f * animSpeed);
        yield return new WaitForSeconds(0.5f * animSpeed);
        state = MachineState.Normal;
    }

    private void ChangePartColor()
    {
        if (heldItem == null) return;

        RobotShell shell = heldItem.GetComponent<RobotShell>();
        if (shell == null) return;
        shell.ChangeColor(currentPaintColor);
    }

    protected override void DisableConnectedMachines()
    {
        foreach (PainterSwitch pb in painterButtons)
        {
            pb.StartCoroutine(pb.DisableMachine(false, false));
        }
    }
}
