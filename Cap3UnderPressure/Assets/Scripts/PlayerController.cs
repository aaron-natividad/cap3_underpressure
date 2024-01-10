using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Collider collision;

    // Controls
    [HideInInspector] public PlayerInputControls controls;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction interactAction;
    [HideInInspector] public InputAction dashAction;

    // States
    [HideInInspector] public bool grounded;

    private void OnEnable()
    {
        SetControls();
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collision = GetComponent<Collider>();
        controls = new PlayerInputControls();
    }

    private void EnableControls()
    {
        moveAction.Enable();
        dashAction.Enable();
        interactAction.Enable();
    }

    private void DisableControls()
    {
        moveAction.Disable();
        dashAction.Disable();
        interactAction.Disable();
    }

    private void SetControls()
    {
        moveAction = controls.Player.Move;
        dashAction = controls.Player.Dash;
        interactAction = controls.Player.Interact;
    }
}
