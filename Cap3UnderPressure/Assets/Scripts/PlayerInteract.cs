using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public PlayerController controller;

    [Header("UI")]
    public Image holdCircle;

    [Header("Interact Parameters")]
    public LayerMask raycastMask;
    public float interactRange;

    private Interactable highlightedItem = null; // Separated reference for highlight
    private Interactable validItem = null;       // Item to be interacted with
    private float pressTime;
    private bool interacted = false;

    void Update()
    {
        GetLastInteractable();

        if (validItem == null)
        {
            pressTime = 0;
            holdCircle.fillAmount = 0;
            interacted = false;
            return;
        }
            
   
        if (controller.interactAction.phase == InputActionPhase.Performed)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= validItem.interactTime)
            {
                if (!interacted)
                {
                    validItem.Interact();
                    interacted = true;
                }
                else
                {
                    holdCircle.fillAmount = 0;
                }
            }
            else if (validItem.interactTime > 0)
            {
                holdCircle.fillAmount = pressTime / validItem.interactTime;
            }
        }
        else
        {
            pressTime = 0;
            holdCircle.fillAmount = 0;
            interacted = false;
        }
    }

    private void GetLastInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, raycastMask))
        {
            // Remove last highlighted item if overlapping
            if (highlightedItem != null && highlightedItem != hit.transform.gameObject.GetComponent<Interactable>())
            {
                highlightedItem.Highlight(false);
                validItem = null;
            }

            // Highlight and store ref
            highlightedItem = hit.transform.gameObject.GetComponent<Interactable>();
            highlightedItem.Highlight(true);

            // Store as valid item if haven't interacted
            if (controller.interactAction.phase != InputActionPhase.Performed && validItem != highlightedItem)
                validItem = highlightedItem;
        }
        else if (highlightedItem != null)
        {
            highlightedItem.Highlight(false);
            highlightedItem = null;
            validItem = null;
        }
    }
}
