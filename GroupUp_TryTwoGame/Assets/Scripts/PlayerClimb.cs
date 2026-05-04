using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerClimb : MonoBehaviour
{
    public enum PlayerState
    {
        WALKING,
        FALLING,
        CLIMBING
    }
    [Header("Player Variables")]
    public PlayerState state;
    public float walkSpeed;
    public float climbSpeed;

    public float horiz;
    public float vert;
    public bool jumpDown = false;
    
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //input happens frame-based
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (!jumpDown)
            jumpDown = Input.GetButtonDown("Jump"); //set a key to jump key in future

        //// check wall directly in front              
        //RaycastHit hit;      // position         direction        hit data
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{
        //    transform.forward = -hit.normal;
        //    rb.position = Vector3.Lerp(rb.position, hit.point + hit.normal * 0.51f, 10f * Time.fixedDeltaTime);
        //}
       
    }

    private void FixedUpdate()
    {
        //prevent diagonal acceleration
        Vector2 rawInput = new Vector2(horiz, vert);
        Vector2 input = Vector2.ClampMagnitude(rawInput, climbSpeed); //???
       // rb.linearVelocity = transform.TransformDirection(input) * climbSpeed; //!!!!

        Transform cam = Camera.main.transform;
        Vector3 moveDirection = Quaternion.FromToRotation(cam.up, Vector3.up)
                                * cam.TransformDirection(new Vector3(input.x, 0f, input.y));

        switch (state)
        {
            case PlayerState.WALKING: { HandleWalking(moveDirection); } break; 
            case PlayerState.FALLING: { HandleFalling(); } break;
            case PlayerState.CLIMBING: { HandleClimbing(input); } break;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position,Vector3.down, out hit, 1.02f))
            state = PlayerState.WALKING;
        else if (state == PlayerState.WALKING)
            state = PlayerState.FALLING;

        rb.useGravity = state != PlayerState.CLIMBING;

        //reset input
        jumpDown = false;
    }

    void HandleWalking(Vector3 moveDirection)
    {
        Vector3 oldVelo = rb.linearVelocity;
        Vector3 newVelo = moveDirection * walkSpeed;
        newVelo.y = oldVelo.y;
        
        if (jumpDown)
        {
            newVelo.y = 5f;
            state = PlayerState.FALLING;
        }

        RaycastHit wallHit;
        if (Physics.Raycast(transform.position, transform.forward, out wallHit))
        {
            state = PlayerState.CLIMBING;
        }
        rb.linearVelocity = newVelo;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDirection;
        }
    }

    void HandleFalling()
    {
        //let player grab wall..?
        if (jumpDown && Physics.Raycast(transform.position, transform.forward, 0.4f))
            state = PlayerState.CLIMBING;

    }

    void HandleClimbing(Vector2 input)
    {

        //check walls in cross pattern
        Vector3 offset = transform.TransformDirection(Vector2.one * 05f);
        Vector3 checkDirection = Vector3.zero;
        int k = 0;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit checkHit;
            if (Physics.Raycast(transform.position + offset, transform.forward, out checkHit) )
            {
                checkDirection += checkHit.normal;
                k++;
            }
            //rotate offset 90 degrees
            offset = Quaternion.AngleAxis(90f, transform.forward) * offset;
        }
        if (k ==0)
        {
            k++;
            //state = PlayerState.FALLING;
        }
        checkDirection /= k;

        //check wall in front
        //get direction of the surfaces's normal and lerp player pos and direction to face normal
         //THATS SO COOL
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -checkDirection, out hit))
        {
            float dot = Vector3.Dot(transform.forward, -hit.normal);

            rb.position = Vector3.Lerp(rb.position, hit.point + hit.normal * 0.55f,
                          5f * Time.fixedDeltaTime);

            transform.forward = Vector3.Lerp(transform.forward, -hit.normal,
                                10f * Time.fixedDeltaTime);

            rb.useGravity = false;
            rb.linearVelocity = transform.TransformDirection(input) * climbSpeed;

            if (jumpDown)
            {
                rb.linearVelocity = Vector3.up * 5f + hit.normal * 2f;
                state = PlayerState.FALLING;
            }
            //else
            //{
            //    state = PlayerState.FALLING;
            //}

        }

    }
}
