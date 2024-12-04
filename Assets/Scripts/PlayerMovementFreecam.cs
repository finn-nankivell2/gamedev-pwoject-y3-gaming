using System;
using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;

public class PlayerMovementFreecam : MonoBehaviour
{
    [Header("Camera")]
    private CharacterController characterController;
    public Transform mainCamera;
    public Transform orientation;

    [Header("Movement")]
    public float maxSpeed = 12;
    public float gravityForce = 0.3f;
    public float rotationSpeed = 16f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public int maxAirJumps = 0;
    private int airJumps;
    public float baseSpeedMod = 1f;
    private float currentAirSpeedMod;
    public float airJumpModAdd = 0.2f;
    public float timeToSlow = 0.5f;
    public float speedModLostOnTouch = 0.4f;
    public float coyoteSeconds = 0.2f;
    private float midairTime;
    private bool hasJumped = false;
    private bool touchedGroundLastFrame = false;

    private Vector3 velocity;
    private float ySpeed;
    private bool jumpStorage = false;

    public GameObject airJumpParticles;
    public Transform particleOrigin;
    private Vector3 startPos;

    [Header("Animation")]
    public Animator animationController;
    public float kickAnimationSlowdownRate = 0.25f;

	private static class AnimationState {
		static public int Idle = 0;
		static public int Run = 1;
		static public int Midair = 2;
		static public int Kick = 3;
		static public int DoubleJump = 4;
	}

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        startPos = transform.position;
        animationController = GetComponent<Animator>();
        currentAirSpeedMod = baseSpeedMod;
    }

    private Vector3 InputDirection() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        return new (horizontalInput, 0, verticalInput);
    }

    // Update is called once per frame
    void Update()
    {
        ySpeed -= gravityForce * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpStorage = true;
        }

        Vector3 inputDirection = InputDirection();

		Vector3 cameraDirection = mainCamera.forward;
		cameraDirection.y = 0f;

		orientation.forward = cameraDirection;
        Vector3 moveDirection = orientation.TransformDirection(inputDirection.normalized);
        Vector3 flatMoveDirection = new (moveDirection.x, 0, moveDirection.z);

        velocity = flatMoveDirection.normalized * maxSpeed;
        velocity.y = ySpeed;

		ModifySpeed();
        characterController.Move(velocity * Time.deltaTime);

		var norm = new Vector3(velocity.normalized.x, 0f, velocity.normalized.z);
		if (norm.magnitude > 0f) {
            transform.forward = Vector3.RotateTowards(transform.forward, norm, rotationSpeed * Time.deltaTime, 0.0f);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			transform.position = startPos;
			Physics.SyncTransforms();
		}

    }

    void FixedUpdate()
    {
        if(characterController.isGrounded)
        {
            ySpeed = 0;
            airJumps = maxAirJumps;
            // currentAirSpeedMod = baseAirSpeedMod;
            midairTime = 0f;
            hasJumped = false;

            if(currentAirSpeedMod > baseSpeedMod) {
                float extraAirSpeed = currentAirSpeedMod - baseSpeedMod;
                if(touchedGroundLastFrame){
                    currentAirSpeedMod -= extraAirSpeed * speedModLostOnTouch;
                }
                currentAirSpeedMod -= extraAirSpeed * Time.deltaTime / timeToSlow;
            }

            // 0: Idle
			animationController.SetInteger("animationState", AnimationState.Idle);

            if(InputDirection().magnitude > 0f){
                // 1: Running
    			animationController.SetInteger("animationState", AnimationState.Run);
            }

			// 3: Kick
			if (Input.GetKey(KeyCode.F)) {
				animationController.SetInteger("animationState", AnimationState.Kick);
			}

            touchedGroundLastFrame = true;
        }

        else {
            // 2: Mid-air
            if(touchedGroundLastFrame) {
                currentAirSpeedMod += airJumpModAdd;
            }
            animationController.SetInteger("animationState", AnimationState.Midair);
            animationController.SetFloat("JumpVelocityBlend", velocity.y);
            midairTime += Time.deltaTime;
            touchedGroundLastFrame = false;
        }

        if(jumpStorage){
            if(characterController.isGrounded || (midairTime < coyoteSeconds && !hasJumped)){
                Jump();
            }
            else{
                AirJump();
            }
            jumpStorage = false;
            hasJumped = true;
        }

    }

    void ModifySpeed()
    {
        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
		float localMaxSpeed = maxSpeed;

		if (animationController.GetCurrentAnimatorStateInfo(0).IsName("metarig|kick")) {
			localMaxSpeed = maxSpeed * kickAnimationSlowdownRate;
		}

        // if (!characterController.isGrounded) {
		// 	localMaxSpeed = maxSpeed * currentAirSpeedMod;
        // }
        localMaxSpeed = maxSpeed * currentAirSpeedMod;

		Vector3 limitedFlatVelocity = flatVelocity.normalized * localMaxSpeed;
		velocity = new (limitedFlatVelocity.x, velocity.y, limitedFlatVelocity.z);
    }

    void Jump()
    {
        ySpeed = jumpForce;
    }

    void AirJump()
    {
        if(airJumps > 0){
			animationController.SetInteger("animationState", AnimationState.DoubleJump);
            ySpeed = jumpForce;
            airJumps -= 1;

            Instantiate(airJumpParticles, transform.position, particleOrigin.rotation);
            // airJumpParticles.Play();
            currentAirSpeedMod += airJumpModAdd;
        }
    }

    public void AddJump()
    {
        maxAirJumps += 1;
        airJumps = maxAirJumps;
    }
}
