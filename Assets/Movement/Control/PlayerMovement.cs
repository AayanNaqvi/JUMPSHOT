using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float knifeWalkSpeed;
    public float sprintSpeed;
    public float knifeSprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float knifeWallRunSpeed;
    public float adsSpeed;

    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float knifeCrouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public GameObject body;
    public GameObject head;
    public GameObject arms;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    
    


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public GameObject gun;
    private AimDownSights ads;
    private GunSystem gs;
    public PlayerCam cam;
    public Animator anim;
    
    public GameObject gunHolder;
    public GameObject playerParent;
    private WeaponSwitching ws;

    Rigidbody rb;

    public MovementState state;

    [Header("Footstep SFX")]
    public AudioClip[] footstepClips; // Assign 6 clips in the Inspector
    public float footstepVolume;
    public float footstepInterval = 0.45f;
    public float sprintInterval;
    public AudioClip jump;
    public float jumpVol;

    private float footstepTimer = 0f;
    private float currInterval;



    public enum MovementState
    {
        idle,
        walking,
        crouching,
        wallrunning,
        sprinting,
        sliding,
        air
    }

    public bool sliding;
    public bool wallrunning;

    private void Start()
    {
        ws = gunHolder.GetComponent<WeaponSwitching>();
        currInterval = footstepInterval;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        if(ws.selectedWeapon != 2)
        {
            ads = gun.GetComponentInChildren<AimDownSights>();
        }
        
        

        readyToJump = true;

        startYScale = body.transform.localScale.y;
        footstepVolume = EditVolume.playerVol;
    }

    private void Update()
    {
        footstepVolume = EditVolume.playerVol;
        jumpVol = EditVolume.playerVol;


        if (LockInputs.inputLocked) return;



        if (state == MovementState.walking && grounded && rb.velocity.magnitude > 1f)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else if(state == MovementState.sprinting && grounded && rb.velocity.magnitude > 1.2f)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = sprintInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // Reset if not walking
        }


        if (ws.selectedWeapon != 2)
        {
            ads = gun.GetComponentInChildren<AimDownSights>();
        }


        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        
        MyInput();
        SpeedControl();
        StateHandler();

        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        

        MovePlayer();
    }

    private void MyInput()
    {
        
        
         horizontalInput = Input.GetAxisRaw("Horizontal");
         verticalInput = Input.GetAxisRaw("Vertical");
        
        

        //when jump;
        if (Input.GetKey(jumpKey) && readyToJump && grounded && !LockInputs.inputLocked)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        


        //Start Crouch
        if (Input.GetKeyDown(crouchKey) && grounded && state != MovementState.sprinting && !LockInputs.inputLocked)
        {
            body.transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            //head.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            
        }
        //End Crouch
        if (Input.GetKeyUp(crouchKey))
        {
            body.transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            //head.transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }


    }

    public void StateHandler()
    {

        

        //Mode - Wallrun

        if (wallrunning)
        {
            anim.SetBool("Run", false);


            if (ws.selectedWeapon == 2)
            {
                state = MovementState.wallrunning;
                desiredMoveSpeed = knifeWallRunSpeed;
            }
            else
            {
                state = MovementState.wallrunning;
                desiredMoveSpeed = wallRunSpeed;
            }



        }

        //Mode - Sliding
        else if (sliding)
        {
            anim.SetBool("Run", false);
            state = MovementState.sliding;




            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }



        //Mode - Sprint
        else if (grounded && Input.GetKey(sprintKey) && verticalInput > 0 && state != MovementState.crouching)
        {
            if (ws.selectedWeapon == 2)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = knifeSprintSpeed;
            }
            else
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }

            anim.SetBool("Run", true);




        }
        //Mode - Crouching
        else if (Input.GetKey(crouchKey) && grounded && state != MovementState.sprinting)
        {
            anim.SetBool("Run", false);

            if (ws.selectedWeapon == 2)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = knifeCrouchSpeed;
            }
            else
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }


        }
        //Mode - Walk
        else if (grounded && rb.velocity.magnitude != 0)
        {
            anim.SetBool("Run", false);

            if (ws.selectedWeapon == 2)
            {
                state = MovementState.walking;
                desiredMoveSpeed = knifeWalkSpeed;
            }
            else
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;

            }



        }

        //Mode - Idle
        else if (grounded)
        {
            anim.SetBool("Run", false);
            state = MovementState.idle;

        }
        //Mode - Air
        else
        {
            anim.SetBool("Run", false);
            state = MovementState.air;


        }

        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 5f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
        

    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        

        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        //calculate Move direction
        moveDirection = orientation.forward  * verticalInput + orientation.right * horizontalInput;
        
        //On Slope
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }


        

        //on ground
        else if(grounded)
        {
            rb.AddForce((moveSpeed * moveDirection) * 10f, ForceMode.Force);
        }
        //in air
        else if(!grounded)
        {
            rb.AddForce((moveSpeed * moveDirection) * 10f * (airMultiplier*2f), ForceMode.Force);
        }
        

        rb.useGravity = !OnSlope();

    }

    private void SpeedControl()
    {
        if(OnSlope() && !exitingSlope)
        {
            if(grounded && rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        
        
        
        
    }

    private void Jump()
    {
        exitingSlope = true;

        
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        AudioManager.instance.Play3DSound(jump, transform.position, jumpVol);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    private void PlayFootstepSound()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[index];

        AudioManager.instance.Play3DSound(clip, transform.position, footstepVolume);
    }
}
