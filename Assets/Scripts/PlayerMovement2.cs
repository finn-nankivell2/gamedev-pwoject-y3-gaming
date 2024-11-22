using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    private CharacterController characterController;
    public Transform mainCamera;

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

    }
}
