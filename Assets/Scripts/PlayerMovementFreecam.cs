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
    public float groundPoundVelocity = 100f;
    private bool groundPounding = false;
    private float timeSinceGroundPound = 1f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public int maxAirJumps = 0;
    public float highJumpModifier = 1.25f;
    public float highJumpWindow = 0.2f;

    [System.NonSerialized]
    public int airJumps;
    public float baseSpeedMod = 1f;
    private float airSpeedMod;
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

    [Header("Particles")]
    public GameObject airJumpParticles;
    public Transform particleOrigin;
    public ParticleSystem residualJumpParticles;
    private Vector3 startPos;

    [Header("Animation")]
    public Animator animationController;
    public Animator outlineAnimationController;
    public float kickAnimationSlowdownRate = 0.25f;

    [Header("Audio")]
	public AudioSource footstepsAudio;

	private static class AnimationState {
		static public int Idle = 0;
		static public int Run = 1;
		static public int Midair = 2;
		static public int Kick = 3;
		static public int DoubleJump = 4;
        static public int GroundPound = 5;
	}

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        startPos = transform.position;
        animationController = GetComponent<Animator>();
        airSpeedMod = baseSpeedMod;
    }

    private Vector3 InputDirection() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        return new (horizontalInput, 0, verticalInput);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            ySpeed = -groundPoundVelocity;
            groundPounding = true;
        }

        if(!groundPounding) {
            ySpeed -= gravityForce * Time.deltaTime;
        }

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


    }

    void FixedUpdate()
    {
        timeSinceGroundPound += Time.deltaTime;

        if(characterController.isGrounded)
        {
            ySpeed = 0;
            airJumps = maxAirJumps;
            midairTime = 0f;
            hasJumped = false;

            if(groundPounding) {
                groundPounding = false;
                timeSinceGroundPound = 0;
                GameManager.Instance.particleManager.Play("groundpound", particleOrigin.position);
                GameManager.Instance.audioManager.Play("groundslam");
            }

            if(airSpeedMod > baseSpeedMod) {
                float extraAirSpeed = airSpeedMod - baseSpeedMod;
                if(!touchedGroundLastFrame){
                    airSpeedMod -= extraAirSpeed * speedModLostOnTouch;
                }
                airSpeedMod -= extraAirSpeed * Time.deltaTime / timeToSlow;
            }

            // 0: Idle
			animationController.SetInteger("animationState", AnimationState.Idle);
			outlineAnimationController.SetInteger("animationState", AnimationState.Idle);

            if(InputDirection().magnitude > 0f){
                // 1: Running
    			animationController.SetInteger("animationState", AnimationState.Run);
    			outlineAnimationController.SetInteger("animationState", AnimationState.Run);

            }

			// 3: Kick
			if (Input.GetKey(KeyCode.F)) {
				animationController.SetInteger("animationState", AnimationState.Kick);
				outlineAnimationController.SetInteger("animationState", AnimationState.Kick);
			}

            touchedGroundLastFrame = true;
        }

        else {
            // 2: Mid-air
            if(touchedGroundLastFrame) {
                airSpeedMod += airJumpModAdd;
            }

            animationController.SetFloat("JumpVelocityBlend", velocity.y);
            outlineAnimationController.SetFloat("JumpVelocityBlend", velocity.y);
            midairTime += Time.deltaTime;

            if(!groundPounding) {
                animationController.SetInteger("animationState", AnimationState.Midair);
                outlineAnimationController.SetInteger("animationState", AnimationState.Midair);
            } else {
                animationController.SetInteger("animationState", AnimationState.GroundPound);
                outlineAnimationController.SetInteger("animationState", AnimationState.GroundPound);
            }

            touchedGroundLastFrame = false;
        }

		if (characterController.isGrounded && InputDirection().magnitude > 0f) {
			if (!footstepsAudio.isPlaying) {
				footstepsAudio.Play();
			}
		}

		else if (footstepsAudio.isPlaying) {
			footstepsAudio.Stop();
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
        if (groundPounding) {
            velocity = new (0, velocity.y, 0);
        }

        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
		float localMaxSpeed = maxSpeed;

		if (animationController.GetCurrentAnimatorStateInfo(0).IsName("metarig|kick")) {
			localMaxSpeed = maxSpeed * kickAnimationSlowdownRate;
		}

        localMaxSpeed = maxSpeed * airSpeedMod;

		Vector3 limitedFlatVelocity = flatVelocity.normalized * localMaxSpeed;
		velocity = new (limitedFlatVelocity.x, velocity.y, limitedFlatVelocity.z);
    }

    void Jump()
    {
        if(timeSinceGroundPound < highJumpWindow) {
            ySpeed = jumpForce * highJumpModifier;
        } else {
            ySpeed = jumpForce;
        }
    }

    void AirJump()
    {
        if(airJumps > 0 && !groundPounding){
			animationController.SetInteger("animationState", AnimationState.DoubleJump);
			outlineAnimationController.SetInteger("animationState", AnimationState.DoubleJump);
            ySpeed = jumpForce;
            airJumps -= 1;

            // Instantiate(airJumpParticles, transform.position, particleOrigin.rotation);
            GameManager.Instance.particleManager.Play("jump", transform.position, particleOrigin.rotation);
            GameManager.Instance.audioManager.Play("whoosh");
            residualJumpParticles.Play();
            airSpeedMod += airJumpModAdd;
        }
    }

    public void AddJump()
    {
        maxAirJumps += 1;
        airJumps = maxAirJumps;
    }

    public void ResetSpeedMod()
    {
        airSpeedMod = baseSpeedMod;
    }
}
