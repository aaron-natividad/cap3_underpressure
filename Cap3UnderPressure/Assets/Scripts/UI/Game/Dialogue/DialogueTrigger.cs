using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueHandler handler;
    [SerializeField] private int groupIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            handler.StartDialogueGroup(groupIndex);
            Destroy(gameObject);
        }
    }
}
