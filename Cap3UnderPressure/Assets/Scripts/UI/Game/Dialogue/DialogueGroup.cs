using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueItem
{
    public Transform focusPoint;
    public float lookTime = 0.5f;
    [Space(10)]
    public string speakerName;
    public AudioClip textSound;
    [Multiline] public string dialogueText;
}

[System.Serializable]
public class DialogueGroup
{
    public DialogueRequirementType requirementType;
    public string requiredItem;
    public string requiredMachine;

    public DialogueItem[] dialogueItems;
}
