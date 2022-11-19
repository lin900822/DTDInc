using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController = null;

    [SerializeField]
    private Transform cameraTransform = null;
    [SerializeField]
    private Transform footTransform = null;

    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;

    [SerializeField]
    private float jumpVelocity = .1f;
    [SerializeField]
    private float gravity = -.35f;

    [SerializeField]
    private float mouseXSensitivity = 300f;
    [SerializeField]
    private float mouseYSensitivity = 300f;

    float rotationX = 0f;
    float rotationY = 0f;
    float velocityY = 0f;
    bool isGround = false;

    private void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");

        Vector3 moveVector = transform.TransformDirection(Vector3.forward * yInput + Vector3.right * xInput).normalized;
        bool isRun = Input.GetKey(KeyCode.LeftShift);
        moveVector *= (isRun ? runSpeed : walkSpeed) * Time.deltaTime;

        isGround = IsGround();

        ApplyGravity();

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }

        moveVector.y = velocityY;

        characterController.Move(moveVector);

        rotationX += mouseXInput * mouseXSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationX, 0);

        rotationY += mouseYInput * mouseYSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -89, 89);

        cameraTransform.localRotation = Quaternion.Euler(-rotationY, cameraTransform.localRotation.y, cameraTransform.localRotation.z);
    }

    private void Jump()
    {
        velocityY = jumpVelocity * Time.deltaTime;
    }

    private void ApplyGravity()
    {
        if (!isGround)
            velocityY += gravity * Time.deltaTime;
    }

    private bool IsGround()
    {
        var colliders = Physics.OverlapSphere(footTransform.position, .2f);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject)
                return true;
        }

        return false;
    }
}