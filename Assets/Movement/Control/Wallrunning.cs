using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wallrunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    //public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit rightWallhit;
    private RaycastHit leftWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting Wall")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement pm;
    private Rigidbody rb;

    [Header("Sound")]
    public AudioClip wallrunSFX;
    public float wrVol;

    

    private void Start()
    {
        
        
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();


    }
    private void Update()
    {
        wrVol = EditVolume.playerVol;


        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        


        if (pm.wallrunning)
        {
            WallRunMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            //Start wallrun
            if (!pm.wallrunning)
            {
                StartWallRun();
            }
            if(Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }
        //State 2 - Exiting

        else if(exitingWall)
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }
            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        //State 3 - None
        else
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        AudioManager.instance.Play3DSound(wallrunSFX, transform.position, wrVol);


        Debug.Log("Cam Effect Applied");
        
        
        cam.DoFov(70f, 0.25f);
        if(wallLeft) cam.DoTilt(-10f, 0.25f);
        
        if (wallRight) cam.DoTilt(10f, 0.25f);
        
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //Force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
        
    }
    private void StopWallRun()
    {
        pm.wallrunning = false;

        
        
        cam.DoFov(60f, 0.25f);
        cam.DoTilt(0f, 0.25f);
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        AudioManager.instance.Play3DSound(pm.jump, transform.position, pm.jumpVol);
        
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }

}