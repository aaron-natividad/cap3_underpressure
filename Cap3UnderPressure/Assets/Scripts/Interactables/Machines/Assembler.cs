using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assembler : Machine
{
    [SerializeField] private AudioClip robotArmSound;
    [Space(10)]
    [SerializeField] private GameObject robotPrefab;

    [Header("Assembler Particles")]
    [SerializeField] private ParticleSystem attachParticle;

    [Header("Assembler Parts")]
    [SerializeField] private GameObject topPart;
    [SerializeField] private GameObject leftHolder;
    [SerializeField] private GameObject leftPart;
    [SerializeField] private GameObject rightHolder;
    [SerializeField] private GameObject rightPart;

    [Header("Attach Points")]
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform robotPoint;

    [Header("Assembler Parameters")]
    [SerializeField] private float topDistance;
    [SerializeField] private float holderDistance;
    [SerializeField] private float partDistance;

    private Item robotItem;
    private Vector3 topPartPos;
    private Vector3 leftHolderPos;
    private Vector3 leftPartPos;
    private Vector3 rightHolderPos;
    private Vector3 rightPartPos;

    protected override void Initialize()
    {
        base.Initialize();
        topPartPos = topPart.transform.localPosition;
        leftHolderPos = leftHolder.transform.localPosition;
        leftPartPos = leftPart.transform.localPosition;
        rightHolderPos = rightHolder.transform.localPosition;
        rightPartPos = rightPart.transform.localPosition;
    }

    public override void Interact(Player player)
    {
        if (state != MachineState.Normal) return;

        bool validRoomba = robotItem != null;
        if (validRoomba) validRoomba = robotItem.GetComponent<Robot>().IsComplete();

        if (player.heldItem == null)
        {
            if (validRoomba)
            {
                GiveItem(player, ref robotItem);
                HoldObject(false, 0.5f * animSpeed);
                OnMachineInteracted?.Invoke(this);
            }
            else if (heldItem != null)
            {
                GiveItem(player, ref heldItem);
                OnMachineInteracted?.Invoke(this);
            }
        }
        else if (heldItem == null)
        {
            TakeItem(player, attachPoint);
            OnMachineInteracted?.Invoke(this);
        }
    }

    public override IEnumerator PerformAction()
    {
        state = MachineState.Performing;

        if (robotItem == null)
        {
            HoldObject(true, 0.5f * animSpeed);
            yield return new WaitForSeconds(0.5f * animSpeed);
            SpawnRoomba();
        }

        audioSource.PlayOneShot(robotArmSound);
        LeanTween.moveLocal(topPart, topPartPos - new Vector3(0, topDistance, 0), 0.2f * animSpeed);
        yield return new WaitForSeconds(0.2f * animSpeed);
        AttachRobotPart();

        LeanTween.moveLocal(topPart, topPartPos, 0.5f * animSpeed).setEase(LeanTweenType.easeOutCubic);
        yield return new WaitForSeconds(0.5f * animSpeed);

        state = storedState;
    }

    private void SpawnRoomba()
    {
        Robot spawnedItem = Instantiate(robotPrefab).GetComponent<Robot>();
        robotItem = spawnedItem;
        robotItem.EnablePhysics(false);
        robotItem.transform.forward = robotPoint.forward;
        robotItem.transform.position = robotPoint.position;
    }

    private void AttachRobotPart()
    {
        if (heldItem == null) return;
        if (!robotItem.GetComponent<Robot>().AttachPart(heldItem)) return;
        attachParticle.Play();
        heldItem = null;
    }

    private void HoldObject(bool isHolding, float delay)
    {
        Vector3 lPos1 = isHolding ? leftHolderPos - new Vector3(0, holderDistance, 0) : leftHolderPos;
        Vector3 lPos2 = isHolding ? leftPartPos - new Vector3(partDistance, 0,0) : leftPartPos;
        Vector3 rPos1 = isHolding ? rightHolderPos - new Vector3(0, holderDistance, 0) : rightHolderPos;
        Vector3 rPos2 = isHolding ? rightPartPos + new Vector3(partDistance, 0, 0) : rightPartPos;

        LeanTween.moveLocal(leftHolder, lPos1, delay).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(rightHolder, rPos1, delay).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(leftPart, lPos2, delay).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocal(rightPart, rPos2, delay).setEase(LeanTweenType.easeOutCubic);
        audioSource.PlayOneShot(robotArmSound);
    }
}
