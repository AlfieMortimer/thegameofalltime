using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public Transform cam;

    public string endscene;

    public static float health = 100;

    public float diveDistance;

    public float speed = 6;
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 5;
    public float jumps;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    public LayerMask enemy;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool isGrounded;
    bool movementlock = false;
     void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (health <= 0)
        {
            movementlock = true;
            anim.SetBool("isAlive", false);
        }



        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        if (isGrounded)
        {
            jumps = 1;
            anim.SetBool("DoubleJump", false);
            anim.SetBool("CanDJ", true);

        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (Input.GetButtonDown("Jump") && isGrounded == true && movementlock == false)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else if (Input.GetButtonDown("Jump") && jumps > 0 && movementlock == false)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumps--;
            anim.SetBool("DoubleJump", true);
            StartCoroutine(wait());      
        }
        if (isGrounded == false)
        {
            anim.SetBool("falling", true);
        }
        else
        {
            anim.SetBool("falling", false);
        }




        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        if (direction.magnitude >= 0.1f && movementlock == false)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("Run", true);

            
        }
        else
        {
            anim.SetBool("Run", false);
        }       
    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("CanDJ", false);

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 6)
        {
            health = health - 25;
        }
        if (collision.gameObject.layer == 8)
        {
            health = 0;
        }
        if (collision.gameObject.layer == 9)
        {
            SceneManager.LoadScene(endscene);
        }
    }
}
