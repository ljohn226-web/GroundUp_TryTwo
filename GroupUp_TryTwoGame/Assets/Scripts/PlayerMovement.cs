using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    //public float walkSpeed;
    //public float sprintSpeed;
    //public float climbSpeed;
    public float wallRunSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Anim Bools")]
    public bool isClimbing;
    public bool isWallRunning;
    public bool isWalking;
    

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    

    public enum PlayerState
    {
        walking,
        //sprinting,
        wallRunning,
        climbing, 
        //inAir
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;    

        if (grounded && jumpCooldown >= 0)
            canJump = true;

    }
        
    void FixedUpdate()
    {
        MovePlayer();

    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput < 0.01 && horizontalInput > -0.01 && verticalInput < 0.01 && verticalInput > -0.01)
            isWalking = false;
         else
            isWalking = true;

        //check jump
        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
       
        // Calculate movement direction based on orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
       
        //on ground
        if (grounded)
              rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void StateHandler()
    {
        //Mode - Wallrunning
        if (isWallRunning)
        {
           // state = PlayerState.wallRunning;
            moveSpeed = wallRunSpeed;
        }
    }

}
