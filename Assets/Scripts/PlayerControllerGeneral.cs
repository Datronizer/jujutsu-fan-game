using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerGeneral : MonoBehaviour
{
    //input fields
    private BasicPlayerInputActions basicPlayerInputActions;
    private InputAction movement;

    //movement fields
    //private Rigidbody rb;
    private CharacterController characterController;

    //[SerializeField] private CharacterController controller;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    private Vector3 forceDirection = Vector3.zero; // This guides the player where to move after a force is applied

    // Other variables

    public float sprintSpeed;
    public bool isSprinting;

    //math-specific variables
    private float moveSpeed = 6.0f;
    private float jumpHeight = 2.0f;
    private float gravityCoefficient = 9.81f;
    private float terminalVelocity = -10f;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 acceleration;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        //rb = this.GetComponent<Rigidbody>();
        characterController = this.GetComponent<CharacterController>();
        basicPlayerInputActions = new BasicPlayerInputActions();
    }

    private void OnEnable()
    {
        basicPlayerInputActions.Player.Jump.performed += DoJump; // No need to constantly check if jump is pressed. It checks when pressed instead.
        movement = basicPlayerInputActions.Player.Movement;
        basicPlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        basicPlayerInputActions.Player.Jump.performed -= DoJump;
        basicPlayerInputActions.Player.Disable();

    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = 0f; //if you're already on the ground, stop falling

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
            characterController.Move(moveSpeed * Time.deltaTime * moveDirectionFromCam.normalized);
        }

        // Artificially applies gravity
        velocity.y -= gravityCoefficient * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    //private void LookAt()
    //{
    //    Vector3 direction = rb.velocity;
    //    direction.y = 0f;

    //    if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
    //        this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
    //    else
    //        rb.angularVelocity = Vector3.zero;
    //}

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (characterController.isGrounded)
        {
            Debug.Log("Jump!!");
            velocity.y = 0;
            velocity.y += Mathf.Sqrt(jumpHeight * 2.0f * gravityCoefficient);
        }
        else
            Debug.Log("You're hitting jump, but you're not touching the ground");

    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2.2f)) // set the float to character displacement + 0.3f. For example, center of a 3.6m capsule is 1.8 so it's 1.8 + 0.3 = 2.1f
            return true;
        else
            return false;
    }

    //// TODO: Figure out why while jumping, if you move, it freezes the jump height. Probably has something to do with the vector 3 we chose.
    //public void DoMove(Vector2 moveDirection)
    //{
    //    //isSprinting = gamepad.leftStickButton.IsPressed();

    //    //sprintSpeed = isSprinting ? 2f : 1f;
    //    //turnSmoothTime = isSprinting ? 0.1f : 0.1f; // will look at this later

    //    Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;

    //    if (direction.x != 0 || direction.z != 0) // prevent auto clipping back to 90deg angle when stopped
    //    {
    //        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

    //        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //        controller.Move(moveSpeed /** sprintSpeed*/ * Time.deltaTime * moveDir.normalized);
    //    }
    //}
}
