using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerGeneral : MonoBehaviour
{
    //input fields
    private BasicPlayerInputActions basicPlayerInputActions;
    private InputAction movement, sprint, attack;

    //movement fields
    //private Rigidbody rb;
    private CharacterController characterController;

    //camera
    [SerializeField] private Camera playerCamera;

    //math-specific variables
    private float moveSpeed = 6.0f;
    private readonly float jumpHeight = 1.0f;
    private readonly float gravityCoefficient = 9.81f;

    private Vector3 moveDirection;
    private Vector3 velocity;

    private readonly float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private float sprintCoefficient;
    private bool isSprinting;

    private void Awake()
    {
        //rb = this.GetComponent<Rigidbody>();
        characterController = this.GetComponent<CharacterController>();
        basicPlayerInputActions = new BasicPlayerInputActions();
    }

    private void OnEnable()
    {
        basicPlayerInputActions.Player.Jump.performed += DoJump; // No need to constantly check if jump is pressed. It checks when pressed instead.
        basicPlayerInputActions.Player.Attack.performed += DoAttack;
        movement = basicPlayerInputActions.Player.Movement;
        sprint = basicPlayerInputActions.Player.Sprint;
        basicPlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        basicPlayerInputActions.Player.Jump.performed -= DoJump;
        basicPlayerInputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        //if you're already on the ground, stop falling
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = 0f;

        isSprinting = sprint.enabled;
        sprintCoefficient = isSprinting ? 2f : 1f;

        // Checks if character is moving
        float horizontal = movement.ReadValue<Vector2>().x;
        float vertical = movement.ReadValue<Vector2>().y;
        moveDirection = new Vector3(horizontal, 0f, vertical);

        // Moves character if there's movement input and turns character to look where they're moving
        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirectionFromCam = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveSpeed * Time.deltaTime * moveDirectionFromCam.normalized * sprintCoefficient);
        }

        // Artificially applies gravity
        velocity.y -= gravityCoefficient * Time.deltaTime;
        characterController.Move(velocity * (float)Math.Exp(Time.deltaTime) * 0.125f);
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (characterController.isGrounded)
        {
            Debug.Log("Jump!!");
            velocity.y += Mathf.Sqrt(jumpHeight * 1.0f * gravityCoefficient);
        }
        else
            Debug.Log("You're hitting jump, but you're not touching the ground");
    }

    //TODO: Have a script for attack chain so that it can be limited. 
    private void DoAttack(InputAction.CallbackContext obj)
    {
        print("Hyah!");
        BroadcastMessage("__TakeDamage", 15f, SendMessageOptions.DontRequireReceiver);
    }
}
