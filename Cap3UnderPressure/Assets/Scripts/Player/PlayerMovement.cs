using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip footstep;
    [SerializeField] private float footstepWalkDelay;
    [SerializeField] private float footstepRunDelay;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float walkAccel;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runAccel;
    [SerializeField] private float decel;

    [Header("Ramp/Step Parameters")]
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float originDistance;
    [SerializeField] private float stepDistance;

    private Player player;
    private PlayerController controller;
    private float currentSpeed = 0;
    private float footstepTime = 0;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        player = Player.instance;
        controller = Player.instance.controller;
    }

    private void FixedUpdate()
    {
        if (player.state != PlayerState.Normal)
        {
            player.rigidBody.velocity = new Vector3(0f, player.rigidBody.velocity.y, 0f);
            return;
        }
        DoMovementBase();
        DoGroundSnapping();
    }

    private void DoMovementBase()
    {
        Vector2 dir = controller.move.ReadValue<Vector2>();                     // Read directional input
        bool dashPressed = controller.dash.phase == InputActionPhase.Performed; // Check dash button
        if (!player.heldItemRunEnabled && player.heldItem != null) dashPressed = false;

        float maxSpeed = dashPressed ? runSpeed : walkSpeed;                    // Update speeds
        float currentAccel = dashPressed ? runAccel : walkAccel;

        if (dir != Vector2.zero)
        {
            // Accelerate to speed and store direction
            moveDirection = (transform.right * dir.x) + (transform.forward * dir.y);
            currentSpeed = Mathf.Min(currentSpeed + currentAccel, maxSpeed);
            if (player.grounded) DoFootsteps(dashPressed);
        }
        else
        {
            // Decelerate
            currentSpeed = Mathf.Max(currentSpeed - decel, 0);
        }

        // Apply velocity
        Vector3 currentVelocity = moveDirection * currentSpeed;
        currentVelocity.y = player.rigidBody.velocity.y;
        player.rigidBody.velocity = currentVelocity;
    }

    private void DoGroundSnapping()
    {
        RaycastHit hit;
        Vector3 currentPos = transform.position;

        if(Physics.Raycast(transform.position, -transform.up, out hit, originDistance + stepDistance, raycastMask))
        {
            currentPos.y = hit.point.y + originDistance;
            transform.position = currentPos;
            player.rigidBody.velocity = new Vector3(player.rigidBody.velocity.x, 0f, player.rigidBody.velocity.z);
            player.grounded = true;
        }
        else
        {
            player.grounded = false;
        }
    }

    private void DoFootsteps(bool dashPressed)
    {
        float delay = dashPressed ? footstepRunDelay : footstepWalkDelay;
        footstepTime += Time.deltaTime;

        if (footstepTime >= delay)
        {
            footstepTime = 0f;
            player.audioSource.pitch = Random.Range(0.6f, 1.4f);
            player.audioSource.PlayOneShot(footstep);
        }
    }
}
