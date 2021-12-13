using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Mathematics;
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    private bool isGrounded = false;
    Vector3 velocity;

    private GameObject mainCamera;
    private bool isStarted=true;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //Debug.Log(x + "," + z);
            //animator.SetBool("Run",z==0?false:true);

            /*float x = joystickMove.Horizontal;
            float z = joystickMove.Vertical;*/
            Vector3 move;
            move = transform.right * x + transform.forward * z;
            if ((Input.GetButtonDown("Jump")) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            controller.Move(move * speed * Time.deltaTime);

        }

    }

}
