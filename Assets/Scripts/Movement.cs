using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f * 6;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 2.0f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    public bool isMoving;

    private Vector3 lastPosition = new Vector3(0, 0, 0);
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground Check 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // Debug ground check
        //Debug.Log("Is Grounded: " + isGrounded);

        // Resetting initial velocity 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump button pressed");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Movement check
        if (lastPosition != gameObject.transform.position && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
