using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject player;
    public GameObject cam;

    [Header("Camera Movement")]
    public float sensX;
    public float sensY;

    [Header("Camera Bob")]
    public float bobWalkSpeed;
    public float bobWalkMagnitude;
    public float bobRunSpeed;
    public float bobRunMagnitude;

    private float rotationY;
    private float rotationX;
    private Vector3 defaultCameraPos;
    private PlayerController controller;
    private float bobTime = 0;

    private void Start()
    {
        controller = player.GetComponent<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        defaultCameraPos = cam.transform.localPosition;
    }

    void Update()
    {
        rotationY += sensX * Input.GetAxis("Mouse X");
        rotationX -= sensY * Input.GetAxis("Mouse Y");
        rotationX = Mathf.Clamp(rotationX, -89f, 89f);

        // Rotate Player Y
        Vector3 parentRotation = player.transform.eulerAngles;
        parentRotation.y = rotationY;
        player.transform.eulerAngles = parentRotation;

        // Rotate Camera X
        transform.eulerAngles = new Vector3(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);

        DoHeadBob();
    }

    private void DoHeadBob()
    {
        float currentBobSpeed = controller.dashAction.phase == InputActionPhase.Performed ? bobRunSpeed : bobWalkSpeed;
        float currentBobMagnitude = controller.dashAction.phase == InputActionPhase.Performed ? bobRunMagnitude : bobWalkMagnitude;

        if (controller.moveAction.ReadValue<Vector2>() != Vector2.zero && controller.grounded)
        {
            Vector3 cameraPos = defaultCameraPos;
            bobTime += currentBobSpeed * Time.deltaTime;
            cameraPos.y = defaultCameraPos.y + Mathf.Sin(bobTime) * currentBobMagnitude;
            cam.transform.localPosition = cameraPos;
        }
        else
        {
            bobTime = 0;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, defaultCameraPos, 0.03f);
        }
    }
}
