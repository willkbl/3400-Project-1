using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    Animator animator;
    Vector3 input, moveDirection;

    public float moveSpeed = 10;
    public float jumpHeight = 4.01f;
    public float gravity = 60.2f;
    public float airControl = 10f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;

        if (controller.isGrounded)
        {
            moveDirection = input;

            //we can jump
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }

        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

        if (animator != null) //if this player is animated
                              //(so it doesn't mess up the existing first person controller)
        {
            if (transform.position.y > -0.9) //hard-coded for now, probably a better way to do this
            {
                animator.SetInteger("animState", 2); //jump
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                animator.SetInteger("animState", 1); //walk
            }
            else
            {
                animator.SetInteger("animState", 0); //idle
            }
        }
    }
}
