using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCamera : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform cam; 

    [Header("Camera Bob")]
    [SerializeField] private float bobWalkSpeed;
    [SerializeField] private float bobWalkMagnitude;
    [SerializeField] private float bobRunSpeed;
    [SerializeField] private float bobRunMagnitude;

    [Header("Camera Shake")]
    [SerializeField] private float shakeMagnitude;

    [HideInInspector] public bool isShaking;
    
    private Player player;
    private PlayerController controller;
    private Vector3 defaultCameraPos;

    private float sensX;
    private float sensY;
    private float rotationY;
    private float rotationX;
    private float bobTime = 0;

    private void OnEnable()
    {
        SensitivitySlider.OnSensitivityChanged += GetSensitivity;
    }

    private void OnDisable()
    {
        SensitivitySlider.OnSensitivityChanged -= GetSensitivity;
    }

    private void Start()
    {
        player = Player.instance;
        controller = Player.instance.controller;
        GetSensitivity();

        Cursor.lockState = CursorLockMode.Locked;
        defaultCameraPos = cam.localPosition;
        isShaking = false;
    }

    void Update()
    {
        if (player.state != PlayerState.Normal) return;
        MoveCamera();
    }

    private void GetSensitivity()
    {
        sensX = PlayerPrefs.HasKey("SensX") ? PlayerPrefs.GetFloat("SensX") : 2f;
        sensY = PlayerPrefs.HasKey("SensY") ? PlayerPrefs.GetFloat("SensY") : 2f;
    }

    private void DoHeadBob()
    {
        float currentBobSpeed = controller.dash.phase == InputActionPhase.Performed ? bobRunSpeed : bobWalkSpeed;
        float currentBobMagnitude = controller.dash.phase == InputActionPhase.Performed ? bobRunMagnitude : bobWalkMagnitude;

        if (controller.move.ReadValue<Vector2>() != Vector2.zero && player.grounded)
        {
            Vector3 cameraPos = cam.localPosition;
            bobTime += currentBobSpeed * Time.deltaTime;
            cameraPos.y = defaultCameraPos.y + Mathf.Sin(bobTime) * currentBobMagnitude;
            cam.localPosition = cameraPos;
        }
        else
        {
            bobTime = 0;
            cam.localPosition = Vector3.Lerp(cam.localPosition, defaultCameraPos, 0.03f);
        }
    }

    private void DoHeadShake()
    {
        Vector3 cameraPos = cam.localPosition;
        cameraPos.x = defaultCameraPos.x + Random.Range(-shakeMagnitude, shakeMagnitude);
        cam.localPosition = cameraPos;
    }

    private void MoveCamera()
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
        if (isShaking) DoHeadShake();
    }

    public void LookAt(Vector3 lookPos, float lookTime)
    {
        Vector3 relativePos = lookPos - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        rotationX = rotation.eulerAngles.x;
        rotationY = rotation.eulerAngles.y;
        LeanTween.rotateY(player.gameObject, rotationY, lookTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.rotateX(gameObject, rotationX, lookTime).setEase(LeanTweenType.easeOutCubic);

        if(rotationX > 89f) rotationX -= 360f;
    }
}
