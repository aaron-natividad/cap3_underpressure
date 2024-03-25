using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    public static Action OnIntroFinish;
    public static Action<int> OnDialogueGroupEnd;
    public static Action OnDialogueEnd;

    public DialogueGroup[] dialogueGroups;

    private Player player;
    private DialogueUI ui;

    private bool inDialogueGroup;
    private int groupIndex;
    private int itemIndex;

    private void OnEnable()
    {
        SceneHandler.OnSceneReady += StartDialogue;
        PlayerInteract.OnDialogueContinue += ContinueDialogue;
        Machine.OnMachineInteracted += EvaluateInteraction;
        Item.OnItemPickup += EvaluateInteraction;
    }

    private void OnDisable()
    {
        SceneHandler.OnSceneReady -= StartDialogue;
        PlayerInteract.OnDialogueContinue -= ContinueDialogue;
        Machine.OnMachineInteracted -= EvaluateInteraction;
        Item.OnItemPickup -= EvaluateInteraction;
    }

    private void Start()
    {
        player = Player.instance;
        ui = PlayerUI.instance.dialogueUI;
        groupIndex = 0;
    }

    public void StartCurrentDialogueGroup()
    {
        StartDialogueGroup(groupIndex);
    }

    public void StartDialogueGroup(int groupIndex)
    {
        player.state = PlayerState.Dialogue;
        inDialogueGroup = true;
        this.groupIndex = groupIndex;
        itemIndex = 0;
        SendDialogueText(this.groupIndex, 0);
    }

    #region Private Methods
    private void StartDialogue()
    {
        if (dialogueGroups[0].requirementType == DialogueRequirementType.Intro)
            StartDialogueGroup(0);
        else
            OnIntroFinish?.Invoke();
    }

    private void ContinueDialogue()
    {
        if (!inDialogueGroup || ui.inAnimation) return;

        itemIndex++;
        if (itemIndex >= dialogueGroups[groupIndex].dialogueItems.Length)
        {
            EndDialogueGroup();
            groupIndex++;
            if (groupIndex >= dialogueGroups.Length) OnDialogueEnd?.Invoke();
            return;
        }

        SendDialogueText(groupIndex, itemIndex);
    }

    private void EndDialogueGroup()
    {
        player.state = PlayerState.Normal;
        inDialogueGroup = false;
        ui.SetEnabled(false);
        OnDialogueGroupEnd?.Invoke(groupIndex);

        if (dialogueGroups[0].requirementType == DialogueRequirementType.Intro)
            OnIntroFinish?.Invoke();
    }

    private void SendDialogueText(int gIndex, int iIndex)
    {
        DialogueItem currentItem = dialogueGroups[gIndex].dialogueItems[iIndex];

        if (currentItem.focusPoint != null) player.fpsCam.LookAt(currentItem.focusPoint.position, currentItem.lookTime);

        ui.SetEnabled(true);
        ui.UpdateText(currentItem.speakerName, currentItem.dialogueText, currentItem.textSound);
    }
    #endregion

    #region Evaluations
    private void EvaluateInteraction(Machine machine)
    {
        if (groupIndex >= dialogueGroups.Length || inDialogueGroup) return;
        if (dialogueGroups[groupIndex].requirementType != DialogueRequirementType.Machine) return;

        bool validInteraction = dialogueGroups[groupIndex].requiredItem == "" || dialogueGroups[groupIndex].requiredItem == null;
        if (!validInteraction && machine.heldItem != null) 
            validInteraction = dialogueGroups[groupIndex].requiredItem == machine.heldItem.id;
        validInteraction = validInteraction && dialogueGroups[groupIndex].requiredMachine == machine.id;

        if (validInteraction) StartDialogueGroup(groupIndex);
    }

    private void EvaluateInteraction(Item item)
    {
        if (groupIndex >= dialogueGroups.Length || inDialogueGroup) return;
        if (dialogueGroups[groupIndex].requirementType != DialogueRequirementType.Item) return;

        if (item.id == dialogueGroups[groupIndex].requiredItem) StartDialogueGroup(groupIndex);
    }
    #endregion
}
