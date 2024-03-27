using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public static event Action OnDialogueContinue;

    [Header("Interact Parameters")]
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float interactRange;

    private Player player;
    private PlayerController controller;

    private Interactable highlightedInteractable = null; // Separated reference for highlight
    private Interactable validInteractable = null;       // Item to be interacted with

    private void Start()
    {
        player = Player.instance;
        controller = Player.instance.controller;
    }

    void Update()
    {
        GetLastInteractable();

        // Switch controls
        if (player.state == PlayerState.Normal)
            DoInteractControls();
        else if (player.state == PlayerState.Dialogue)
            DoDialogueControls();
    }

    private void DoInteractControls()
    {
        // Drop Controls
        if (controller.drop.WasPressedThisFrame() && player.heldItem != null)
            DropItem();

        // Interact Controls
        UseValidInteractable();
    }

    private void DoDialogueControls()
    {
        // Continue to next text
        if (controller.interact.WasPressedThisFrame())
            OnDialogueContinue?.Invoke();
    }

    private void DropItem()
    {
        RaycastHit hit;
        Item item = player.heldItem;
        player.heldItem.Drop(player);

        if (Physics.SphereCast(transform.position - transform.forward, 0.5f, transform.forward, out hit, 2f))
        {
            item.transform.position = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
        }
    }

    private void GetLastInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, raycastMask))
        {
            // Remove last highlighted item if overlapping
            if (highlightedInteractable != null && highlightedInteractable != hit.transform.gameObject.GetComponent<Interactable>())
            {
                highlightedInteractable.Highlight(false);
                validInteractable = null;
            }

            // Highlight and store ref
            highlightedInteractable = hit.transform.gameObject.GetComponent<Interactable>();
            highlightedInteractable.Highlight(true);

            // Store as valid item if haven't interacted
            if (controller.interact.phase != InputActionPhase.Performed && validInteractable != highlightedInteractable)
                validInteractable = highlightedInteractable;
        }
        else if (highlightedInteractable != null)
        {
            highlightedInteractable.Highlight(false);
            highlightedInteractable = null;
            validInteractable = null;
        }
    }

    private void UseValidInteractable()
    {
        if (validInteractable == null) return;

        if (controller.interact.WasPressedThisFrame())
        {
            validInteractable.Interact(player);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - transform.forward, 0.5f);
    }
}
