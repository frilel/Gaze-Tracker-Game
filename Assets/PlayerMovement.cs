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
    public Transform benchPos;
    public Transform lookAtPoint;
    public bool isSittingDown = false;
    private bool hasSatDown=false;
    private bool canMove=true;

    private GameObject mainCamera;
    private GameManager gameManager;
    private Animator animator;
    private AudioManager audioManager;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main.gameObject;
        gameManager = FindObjectOfType<GameManager>();
        animator=GetComponent<Animator>();
        animator.enabled=false;
        audioManager=FindObjectOfType<AudioManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted)
        {
            if (!isSittingDown&&canMove)
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
                if(move!=Vector3.zero&&!audioManager.walking.isPlaying)
                {
                    audioManager.walking.Play();
                }
                else if(move==Vector3.zero)
                {
                    audioManager.walking.Stop();
                }
                controller.Move(velocity * Time.deltaTime);

                controller.Move(move * speed * Time.deltaTime);
            }
            else if(!hasSatDown)
            {
                GetComponent<CharacterController>().enabled=false;
                //transform.position = Vector3.Lerp(transform.position, benchPos.position, 0.08f);
                transform.position=benchPos.position;
                transform.LookAt(new Vector3(lookAtPoint.position.x,transform.position.y,lookAtPoint.position.z));
                mainCamera.transform.localEulerAngles=Vector3.zero;
                
                //rotateTo(lookAtPoint);
            }
            else
            {

            }

        }


    }
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
        if(!audioManager.shoot.isPlaying)
        {
            audioManager.shoot.Play();
        }
    }
    public void DrawGun()
    {
        animator.enabled=true;
        animator.SetTrigger("DrawGun");
        if(!audioManager.drawGun.isPlaying)
        {
            audioManager.drawGun.Play();
        }
    }
    public void GetShot()
    {
        animator.enabled=true;
        animator.SetTrigger("PlayerDeath");
         StartCoroutine(gameManager.GameOver(2));
    }
    public void SitOnBench()
    {
        isSittingDown = true;
        if(!audioManager.newspaper.isPlaying)
        {
            audioManager.newspaper.Play();
        }
    }
    /*void rotateTo(Transform target)
    {
        Quaternion raw_rotation;
        // ?????????????????????
        Quaternion lookat_rotation;
        // ????????????(?????????????????????)  
        float per_second_rotate = 540.0f;
        // ??????????????????, lerp??????????????????????????? 
        float lerp_speed = 0.0f;
        // lerp???????????????
        float lerp_tm = 0.0f;

        raw_rotation = transform.rotation;
        // ??????????????????
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        lookat_rotation = transform.rotation;
        // ??????????????????
        transform.rotation = raw_rotation;
        // ??????????????????
        float rotate_angle = Quaternion.Angle(raw_rotation, lookat_rotation);
        // ??????lerp??????
        lerp_speed = per_second_rotate / rotate_angle;
        lerp_tm = 0.0f;
        lerp_tm += Time.deltaTime * lerp_speed;
        transform.rotation = Quaternion.Lerp(raw_rotation, lookat_rotation, lerp_tm);
        if (lerp_tm >= 1)
        {
            transform.rotation = lookat_rotation;
            isSittingDown=false;
            canMove=false;
            hasSatDown=true;
            // ??????, ????????????, ????????????????????????
        }
    }*/

}
