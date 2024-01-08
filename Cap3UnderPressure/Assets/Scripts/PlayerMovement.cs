using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float moveAcceleration;

    // Components
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public Collider2D collision;

    // Controls
    [HideInInspector] public PlayerInputControls controls;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction interactAction;
    [HideInInspector] public InputAction dashAction;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collision = GetComponent<Collider2D>();
        controls = new PlayerInputControls();
    }

    private void FixedUpdate()
    {
        if(moveAction.ReadValue<Vector2>() != Vector2.zero)
        rigidBody.velocity = moveAction.ReadValue<Vector2>() * moveSpeed;
    }

    private void OnEnable()
    {
        SetControls();
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
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
