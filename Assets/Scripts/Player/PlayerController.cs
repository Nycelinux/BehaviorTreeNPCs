using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public Transform cameraTransform;
    private bool IsGrounded;
    public bool canLock = true;
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
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueUI.instance != null &&
        DialogueUI.instance.dialoguePanel.activeSelf)
            return;
        if (canLock)
            HandleMouseLook();
        HandleMovement();
        HandleJump();
        AddGravity();
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LEFT CLICK");
        }
    }

    void HandleMovement()
    {
        if (DialogueUI.instance != null &&
        DialogueUI.instance.dialoguePanel.activeSelf)
            return;
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.A))
            x = -1f;

        if (Input.GetKey(KeyCode.D))
            x = 1f;

        if (Input.GetKey(KeyCode.W))
            z = 1f;

        if (Input.GetKey(KeyCode.S))
            z = -1f;

        transform.Rotate(0f, x * 150f * Time.deltaTime, 0f);

        Vector3 move = transform.forward * z;

        Vector3 finalMove = move * speed;
        finalMove.y = velocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }

    void HandleJump()
    {
        if (DialogueUI.instance != null &&
        DialogueUI.instance.dialoguePanel.activeSelf)
            return;
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetButtonDown("Jump") && IsGrounded)
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
        if (DialogueUI.instance != null &&
        DialogueUI.instance.dialoguePanel.activeSelf)
            return;
        float lookUp = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            lookUp = 1f;

        if (Input.GetKey(KeyCode.DownArrow))
            lookUp = -1f;

        xRotation -= lookUp * 60f * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
