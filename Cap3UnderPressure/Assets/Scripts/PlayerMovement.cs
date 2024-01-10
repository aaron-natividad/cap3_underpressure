using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float walkAccel;
    public float runSpeed;
    public float runAccel;
    public float decel;

    [Header("Ramp/Step Parameters")]
    public LayerMask raycastMask;
    public float originDistance;
    public float stepDistance;

    private PlayerController controller;
    private float currentSpeed = 0;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        DoMovementBase();
        DoGroundSnapping();
    }

    private void DoMovementBase()
    {
        Vector2 dir = controller.moveAction.ReadValue<Vector2>();                     // Read directional input
        bool dashPressed = controller.dashAction.phase == InputActionPhase.Performed; // Check dash button
        float maxSpeed = dashPressed ? runSpeed : walkSpeed;                          // Update speeds
        float currentAccel = dashPressed ? runAccel : walkAccel;

        if (dir != Vector2.zero)
        {
            // Accelerate to speed and store direction
            moveDirection = (transform.right * dir.x) + (transform.forward * dir.y);
            currentSpeed = Mathf.Min(currentSpeed + currentAccel, maxSpeed);
        }
        else
        {
            // Decelerate
            currentSpeed = Mathf.Max(currentSpeed - decel, 0);
        }

        // Apply velocity
        Vector3 currentVelocity = moveDirection * currentSpeed;
        currentVelocity.y = controller.rigidBody.velocity.y;
        controller.rigidBody.velocity = currentVelocity;
    }

    private void DoGroundSnapping()
    {
        RaycastHit hit;
        Vector3 currentPos = transform.position;

        if(Physics.Raycast(transform.position, -transform.up, out hit, originDistance + stepDistance, raycastMask))
        {
            currentPos.y = hit.point.y + originDistance;
            transform.position = currentPos;
            controller.rigidBody.velocity = new Vector3(controller.rigidBody.velocity.x, 0f, controller.rigidBody.velocity.z);
            controller.grounded = true;
        }
        else
        {
            controller.grounded = false;
        }
    }
}
