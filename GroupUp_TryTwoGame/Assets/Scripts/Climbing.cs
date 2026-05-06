using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Climbing : MonoBehaviour
{
    [Header ("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    public bool isClimbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    public float wallLookAngle;
    public float crossPatternOffset = 0.5f;
    public float wallStickDistance = 0.55f;
    public float positionLerpSpeed = 5f;
    public float rotationLerpSpeed = 10f;

    private RaycastHit frontWallHit;
    private bool wallFront;
    private float verticalInput;
    private float horizontalInput;
    private bool jumpDown;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        WallCheck();
        StateMachine();

        if (isClimbing)
            Climb();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (!jumpDown)
            jumpDown = Input.GetButtonDown("Jump");
    }

    private void StateMachine() //STATEMACHINE
    {
        //state 1 - Start climbing
        if (wallFront && wallLookAngle < maxWallLookAngle && climbTimer > 0 && !isClimbing)
        {
            StartClimb();
        }

        //state 2 - Stop climbing (wall gone or timer expired)
        if (isClimbing && (!wallFront || climbTimer <= 0))
        {
            StopClimb();
        }

        //decrement climb timer when climbing
        if (isClimbing)
            climbTimer -= Time.deltaTime;

        //reset timer when grounded
        if (pm.grounded)    
            climbTimer = maxClimbTime;
    }

    private void WallCheck()
    {
        //check wall directly in front
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, 
                orientation.forward, out frontWallHit, detectionLength, whatIsWall);

        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        
            if (pm.grounded)    
            climbTimer = maxClimbTime;
    }

    private void StartClimb()
    {
        isClimbing = true;
        pm.isClimbing = isClimbing;
        //add animator switch here
        rb.useGravity = false;

        //change cam FOV 
    }

    private void Climb()
    {
        //check walls in cross pattern
        Vector3 offset = transform.TransformDirection(Vector2.one * crossPatternOffset);
        Vector3 checkDirection = Vector3.zero;
        int wallCount = 0;

        for (int i = 0; i < 4; i++)
        {
            RaycastHit checkHit;
            if (Physics.Raycast(transform.position + offset, orientation.forward, out checkHit, detectionLength, whatIsWall))
            {
                checkDirection += checkHit.normal;
                wallCount++;
            }
            //rotate offset 90 degrees
            offset = Quaternion.AngleAxis(90f, orientation.forward) * offset;
        }

        if (wallCount == 0)
        {
          
            wallCount = 1;
            StopClimb();
            return;
        }

        checkDirection /= wallCount;

        //check wall in front and stick player to it
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -checkDirection, out hit, detectionLength, whatIsWall))
        {
            //position player at wall surface
            rb.position = Vector3.Lerp(rb.position, hit.point + hit.normal * wallStickDistance,
                          positionLerpSpeed * Time.deltaTime);

            //rotate player to face wall normal
            transform.forward = Vector3.Lerp(transform.forward, -hit.normal,
                                rotationLerpSpeed * Time.deltaTime);

            //climb based on input relative to wall
            Vector3 climbDirection = transform.TransformDirection(new Vector3(horizontalInput, verticalInput, 0f));
            rb.linearVelocity = climbDirection * climbSpeed;

            //jump away from wall
            if (jumpDown)
            {
                rb.linearVelocity = Vector3.up * 5f + hit.normal * 2f;
                StopClimb();
            }
        }
        else
        {
            StopClimb();
        }

        //reset input
        jumpDown = false;
    }

    private void StopClimb()
    {
        rb.useGravity = true;
        isClimbing = false;
        pm.isClimbing = isClimbing;

        //particles on stop
    }
}
