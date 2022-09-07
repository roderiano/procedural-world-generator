using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Animator animator;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    static float OCEAN_POSITION_Y = 3.8f;



    // Start is called before the first frame update
    void Start() 
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update() 
    {

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * walkingSpeed * Input.GetAxis("Vertical")) + (right * walkingSpeed * Input.GetAxis("Horizontal"));
            
            // Set anim flag before apply gravity
            animator.SetBool("IsMoving", moveDirection != Vector3.zero ? true : false);
            animator.SetBool("IsSwimming",  transform.position.y < OCEAN_POSITION_Y);

            if (Input.GetButton("Jump") && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if(transform.position.y < OCEAN_POSITION_Y)
            {
                moveDirection.y = 0f;
            }

            characterController.Move(moveDirection * Time.deltaTime);
    }   
}