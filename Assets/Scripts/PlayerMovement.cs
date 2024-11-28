using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public float maxSpeed = 12;

    private Vector3 velocity;
    private float ySpeed;
    public Transform mainCamera;
    public float gravityForce = 0.3f;
    public float jumpForce = 8f;

    public Animator animationController;

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ySpeed -= gravityForce;
        if(characterController.isGrounded)
        {
            ySpeed = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ySpeed = jumpForce;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new (horizontalInput, 0, verticalInput);

        Vector3 moveDirection = mainCamera.TransformDirection(inputDirection);
        Vector3 flatMoveDirection = new (moveDirection.x, 0, moveDirection.z);

        velocity = flatMoveDirection * maxSpeed;
        velocity.y = ySpeed;

        LimitSpeed();
        characterController.Move(velocity * Time.deltaTime);

		var norm = new Vector3(velocity.normalized.x, 0f, velocity.normalized.z);
		if (norm.magnitude > 0f) {
			transform.forward = norm;
			animationController.SetBool("isRunning", true);
		}

		else {
			animationController.SetBool("isRunning", false);
		}
    }

    void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(velocity.x, 0, velocity.z);
        if(flatVelocity.magnitude > maxSpeed){
            Vector3 limitedFlatVelocity = flatVelocity.normalized * maxSpeed;
            velocity = new (limitedFlatVelocity.x, velocity.y, limitedFlatVelocity.z);
        }
    }
}
