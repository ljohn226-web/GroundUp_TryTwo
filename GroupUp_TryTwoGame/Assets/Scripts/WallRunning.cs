using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRun Vars")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float wallRunForce;
    public float maxRunTime;
    private float runTimer;

    [Header ("Input")]
    private float horzInput;
    private float vertInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;


    [Header("References")]
    private Rigidbody rb;
    public Transform orientation;
    private PlayerMovement pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        StateMachine();
        if (pm.isWallRunning)
        {
            WallRunMovement();
        }
    }

    private void CheckForWall()
    {
                                  //start point of ray, direction of ray, info of hit, distance of hit, layer to sense
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        //USE THIS FOR DEADLY GROUND AND GETTING UP FROM CLIMB
        //raycast down to sense for ground, 
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine() //STATEMACHINE
    {
        //get inputa
        horzInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        //Wall Running
        if((wallLeft || wallRight) && vertInput > 0 && AboveGround())
        {
            if (!pm.isWallRunning)
            {
                //start wall run
                StartWallRun();
            }
        }
        //Nonestate 
        else
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.isWallRunning = true;
    }

    private void WallRunMovement()
    {
        //temporarily disable gravity
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //Find normal of wall
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForwards = Vector3.Cross(wallNormal, Vector3.up);

        //movement speed controlled in PM, add force here
        rb.AddForce(wallForwards * wallRunForce, ForceMode.Force);

    }

    private void StopWallRun()
    {
        pm.isWallRunning = false;
        rb.useGravity = true;
    }

    //to find forward direcrion of the wall, use Vector3.Cross
    //gives you the wall normal upwards orientation, then calculates forward direction of wall

}
