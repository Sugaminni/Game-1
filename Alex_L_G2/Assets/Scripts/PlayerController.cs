using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform cameraPivot;

    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 7.5f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minPitch = -75f;
    [SerializeField] private float maxPitch = 75f;

    [Header("Jump/Gravity")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -22f;

    CharacterController cc;
    Vector2 moveInput;
    Vector2 lookInput;
    float pitch;
    float yVel;
    bool sprintHeld;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Look
        float yaw = lookInput.x * mouseSensitivity;
        float pitchDelta = -lookInput.y * mouseSensitivity;
        transform.Rotate(0f, yaw, 0f);
        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        if (cameraPivot) cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);

        // Move
        Vector3 input = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 world = transform.TransformDirection(input.normalized);
        float speed = (sprintHeld ? sprintSpeed : walkSpeed);

        // Grounding
        if (cc.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;
        Vector3 vel = world * speed + Vector3.up * yVel;
        cc.Move(vel * Time.deltaTime);
    }

    // Input System callbacks 
    void OnMove(InputValue v) => moveInput = v.Get<Vector2>();
    void OnLook(InputValue v) => lookInput = v.Get<Vector2>();
    void OnSprint(InputValue v) => sprintHeld = v.isPressed;
    void OnJump(InputValue v)
    {
        if (!v.isPressed) return;
        if (cc.isGrounded) yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
