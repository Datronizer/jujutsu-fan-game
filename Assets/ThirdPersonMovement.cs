using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public Transform cam;
    public Transform characterObject;

    public float moveSpeed = 6f;
    public float sprintSpeed;

    public bool isSprinting;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    private Vector3 moveDirection;

    private void Awake()
    {
        //playerControls = new PlayerInput();
    }

    private void OnEnable()
    {
        //move = playerControls.Player.Move;
        //move.Enable();
    }

    private void OnDisable()
    {
        //move.Enable();
    }

    private void Update()
    {
        //moveDirection = move.ReadValue<Vector2>();
    }

    //void BasicAttack(Gamepad gamepad)
    //{

    //}

    //void OnLockActivate()
    //{
    //    GameObject lockOnCaster = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    lockOnCaster.transform.SetParent(characterObject);
    //}

    //public void Move(Gamepad gamepad)
    //{
    //    Vector2 move = gamepad.leftStick.ReadValue();
    //    isSprinting = gamepad.leftStickButton.IsPressed();

    //    sprintSpeed = isSprinting ? 2f : 1f;
    //    turnSmoothTime = isSprinting ? 0.1f : 0.1f; // will look at this later

    //    Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;

    //    if (direction.x != 0 || direction.z != 0) // prevent auto clipping back to 90deg angle when stopped
    //    {
    //        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //        controller.Move(speed * sprintSpeed * Time.deltaTime * moveDir.normalized);
    //    }
    //}

    public void Move(Vector2 moveDirection)
    {
        //isSprinting = gamepad.leftStickButton.IsPressed();

        sprintSpeed = isSprinting ? 2f : 1f;
        turnSmoothTime = isSprinting ? 0.1f : 0.1f; // will look at this later

        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;

        if (direction.x != 0 || direction.z != 0) // prevent auto clipping back to 90deg angle when stopped
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveSpeed * sprintSpeed * Time.deltaTime * moveDir.normalized);
        }
    }

    //void Skill(Gamepad gamepad)
    //{
    //    //TODO: Please dont yandereDev this

    //    var yButton = gamepad.yButton.IsPressed();
    //    var upDPad = gamepad.dpad.up.IsPressed();
    //    var downDPad = gamepad.dpad.down.IsPressed();

    //    if (gamepad.leftTrigger.IsPressed())
    //    {
    //        print("you're holding skill button");
    //        if (yButton)
    //        {
    //            print("You casted skill 1");
    //        }
    //        else if (upDPad)
    //        {
    //            if (yButton)
    //            {
    //                print("You casted skill 2");
    //            }
    //        }
    //        else if (downDPad)
    //        {
    //            if (yButton)
    //            {
    //                print("You casted skill 3");
    //            }
    //        }
    //    }
    //}
}
