using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    private CharacterController characterController;
    public Transform mainCamera;
    public Transform orientation;

    public float maxSpeed = 12;
    public float gravityForce = 0.3f;
    public float jumpForce = 8f;

    private Vector3 velocity;
    private float ySpeed;

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

        if(Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            ySpeed = jumpForce;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new (horizontalInput, 0, verticalInput);

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
			transform.forward = norm;
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
