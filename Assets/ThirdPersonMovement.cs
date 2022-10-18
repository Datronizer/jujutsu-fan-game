using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public Transform characterObject;

    public float speed = 6f;
    public float sprintSpeed;

    public bool isSprinting;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    private void Update()
    {
        var gamepad = Gamepad.current;
        // Return null if gamepad not plugged in
        if (gamepad == null)
            return;

        /**
         * Movement and rotations
         */
        Move(gamepad);
    }

    void Attack()
    {

    }

    void OnLockActivate()
    {
        GameObject lockOnCaster = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lockOnCaster.transform.SetParent(characterObject);
    }

    void Move(Gamepad gamepad)
    {
        Vector2 move = gamepad.leftStick.ReadValue();
        isSprinting = gamepad.leftStickButton.IsPressed();

        sprintSpeed = isSprinting ? 2f : 1f;
        turnSmoothTime = isSprinting ? 0.1f : 0.1f; // will look at this later

        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;

        if (direction.x != 0 || direction.z != 0) // prevent auto clipping back to 90deg angle when stopped
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(speed * sprintSpeed * Time.deltaTime * moveDir.normalized);
        }
    }
}
