using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float walkSpeed = 10F;
    public float sprintSpeed = 15F;
    public float jumpSpeed = 0.5F;
    public float gravity = -0.15F;

    private float gravityB;
    private float speedX;
    private float speedZ;
    private float speed;
    private Vector3 move; // Can be improved

    public CharacterController controller;

    void Start()
    {
        speed = walkSpeed;
        gravityB = gravity;
    }

    void Update()
    {
        speedX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        speedZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        move = transform.right * speedX
            + transform.up * gravity
            + transform.forward * speedZ;

        controller.Move(move);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            gravity = gravityB;
            gravity *= -jumpSpeed;
        }

        if (gravity > gravityB)
        {
            gravity -= jumpSpeed * Time.deltaTime;
        }
    }
}
