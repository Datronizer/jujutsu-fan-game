using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Test : MonoBehaviour
{
    //input fields
    private NanamiInputActions nanamiInputActions;
    private InputAction movement;

    //movement fields
    private Rigidbody rb;

    //[SerializeField] private CharacterController controller;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;

    private Vector3 forceDirection = Vector3.zero; // This guides the player where to move after a force is applied

    [SerializeField] private Camera playerCamera;

    //public Transform cam;

    // Other variables
    //public float moveSpeed = 6f;
    [SerializeField] private float gravityCoefficient = 5f;
    public float sprintSpeed;
    public bool isSprinting;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        nanamiInputActions = new NanamiInputActions();
    }

    private void OnEnable()
    {
        nanamiInputActions.Player.Jump.performed += DoJump; // No need to constantly check if jump is pressed. It checks when pressed instead.
        movement = nanamiInputActions.Player.Movement;
        nanamiInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        nanamiInputActions.Player.Jump.performed -= DoJump;
        nanamiInputActions.Player.Disable();

    }

    private void FixedUpdate()
    {
        // Grabs force direction and adds them to character. Resets to zero if you let go of movement.
        forceDirection += movement.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += movement.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        // Accelerates you more so you hit the ground faster. Look up articles on jumping in video games.
        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime * gravityCoefficient; // gravity coefficient multiples the current gravity. Makes things fall faster and harder.

        // Limits the horizontal velocity to maxSpeed. Similarly, you can cap vertical speed.
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > Math.Pow(maxSpeed, 2)) // Pythagorean theorem. Basically if sqrt(velocity) > maxSpeed^2 => cap it to maxSpeed
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y; 

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

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
        print(IsGrounded());
        if (IsGrounded())
        {
            Debug.Log("Jump!!");
            forceDirection += Vector3.up * jumpForce;
        }
        else
            Debug.Log("You're hitting jump, but you're not touching the ground");

    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2.1f)) // set the float to character displacement + 0.3f. For example, center of a 3.6m capsule is 1.8 so it's 1.8 + 0.3 = 2.1f
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
