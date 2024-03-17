using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerInputControls controls;
    [HideInInspector] public InputAction move;
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction drop;
    [HideInInspector] public InputAction dash;

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
        controls = new PlayerInputControls();
    }

    private void EnableControls()
    {
        move.Enable();
        dash.Enable();
        interact.Enable();
        drop.Enable();
    }

    private void DisableControls()
    {
        move.Disable();
        dash.Disable();
        interact.Disable();
        drop.Enable();
    }

    private void SetControls()
    {
        move = controls.Player.Move;
        dash = controls.Player.Dash;
        interact = controls.Player.Interact;
        drop = controls.Player.Drop;
    }
}
