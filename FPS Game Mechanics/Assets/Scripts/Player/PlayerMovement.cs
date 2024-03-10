using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump = true;

    public KeyCode jumpKey = KeyCode.Space;

    public float playerHeight;
    public float groundDrag;
    public LayerMask groundMask;
    bool isGrounded;

    public float moveSpeed;

    public Transform orientation;

    float horizontalInput;
    float VerticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 2f, groundMask);

        SpeedControl();

        horizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        //jump Input
        if(Input.GetKey(jumpKey) && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

        if (isGrounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * VerticalInput + orientation.right * horizontalInput;

        if(isGrounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!isGrounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVelo = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelo.magnitude > moveSpeed)
        {
            Vector3 limitLevel = flatVelo.normalized * moveSpeed;
            rb.velocity = new Vector3(limitLevel.x, rb.velocity.y, limitLevel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        canJump = true;
    }
}
