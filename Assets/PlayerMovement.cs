using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference jumpAction;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Look")]
    public float lookSensitivity = 120f; // degrees per second

    private CharacterController controller;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleGravityAndJump();
    }

    void HandleLook()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        // Horizontal look (yaw only)
        float yaw = lookInput.x * lookSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up, yaw, Space.World);
    }

    void HandleMovement()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleGravityAndJump()
    {
        if (controller.isGrounded)
        {
            if (jumpAction.action.WasPressedThisFrame())
            {
                velocity.y = jumpForce;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}