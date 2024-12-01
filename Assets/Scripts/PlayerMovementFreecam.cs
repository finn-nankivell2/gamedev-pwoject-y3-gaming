using System.Collections;
using System.Collections.Generic;
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

    private Vector3 velocity;
    private float ySpeed;
    private bool jumpStorage = false;

    public ParticleSystem airJumpParticles;
    private Vector3 startPos;

    [Header("Animation")]
    public Animator animationController;
    public float kickAnimationSlowdownRate = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        startPos = transform.position;
        animationController = GetComponent<Animator>();
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

        LimitSpeed();
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

            // 0: Idle
			animationController.SetInteger("animationState", 0);

            if(InputDirection().magnitude > 0f){
                // 1: Running
    			animationController.SetInteger("animationState", 1);
            }

			if (Input.GetKey(KeyCode.F)) {
				animationController.SetInteger("animationState", 3);
			}
        }

        else {
            // 2: Mid-air
            animationController.SetInteger("animationState", 2);
            animationController.SetFloat("JumpVelocityBlend", velocity.y);
        }

        // Debug.LogFormat("animationState: {0}",
        //     animationController.GetInteger("animationState")
        // );

        if(jumpStorage){
            if(characterController.isGrounded){
                Jump();
            }
            else{
                AirJump();
            }
            jumpStorage = false;
        }

    }

    void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
        float localMaxSpeed = (animationController.GetCurrentAnimatorStateInfo(0).IsName("metarig|kick")) ? maxSpeed * kickAnimationSlowdownRate : maxSpeed;
        if(flatVelocity.magnitude > localMaxSpeed) {
            Vector3 limitedFlatVelocity = flatVelocity.normalized * localMaxSpeed;
            velocity = new (limitedFlatVelocity.x, velocity.y, limitedFlatVelocity.z);
        }
    }

    void Jump()
    {
        ySpeed = jumpForce;
    }

    void AirJump()
    {
        if(airJumps > 0){
            ySpeed = jumpForce;
            airJumps -= 1;
            airJumpParticles.Play();
        }
    }

    public void AddJump()
    {
        maxAirJumps += 1;
    }
}
