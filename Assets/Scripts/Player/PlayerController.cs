using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public Transform cameraTransform;
    private bool IsGrounded;
    private Vector3 velocity;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;

    public float speed = 3f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 90f;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
        HandleMovement(); 
        HandleJump(); 
        AddGravity();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.Rotate(0f, x * 150f * Time.deltaTime, 0f);
      
        Vector3 move = transform.forward * z;
        Vector3 finalMove = move * speed + Vector3.up * velocity.y;
        controller.Move(finalMove * speed * Time.deltaTime);

    }

    void HandleJump()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
         if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
         if(Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void AddGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
