using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    //simple movement 
    private CharacterController controller;

    public float speed = 6;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform cam;

    //Dash & Movement
    public Vector3 moveDir;
    public float dashSpeed = 50f;
    public float dashTime = 0.05f;
    public float dashInterval = 2f;
    private float timer;
    public Transform trail;

    //gravity
    public float gravity = 9.8f;
    private float verticalSpeed;

    void Start()
    {
        instance = this;

        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //move
        BasicMovement();

        //gravity
        Gravity();

        //dash
        Dash();

    }

    public void BasicMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (dir.magnitude >= 0.1f)
        {
            //smoothly rotation
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward + new Vector3(0, verticalSpeed, 0);

            //move
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    public void Gravity()
    {
         bool isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            // 在地面上时，重置垂直速度
            verticalSpeed = 0f;
        }
        else
        {
            // 在空中时应用重力
            verticalSpeed -= gravity * Time.deltaTime;
        }
        if(verticalSpeed < -10f)
        {
            verticalSpeed = -10f;
        }

    }
    public void Dash()
    {
         
        timer += Time.deltaTime;
        

        if (Input.GetKeyDown(KeyCode.Space) && timer > dashInterval)
        {
            
            timer = 0;
            trail.gameObject.SetActive(true);
            StartCoroutine(DashForPeriod());
            AudioManager.instance.DashAudio();
           
        }
        if (Input.GetKeyUp(KeyCode.Space) && timer < dashInterval)
        {
            trail.gameObject.SetActive(false);
        }
    }


    public IEnumerator DashForPeriod()
    {
        //prepare for add some delay

        float startTimer = Time.time;

        while (Time.time < startTimer + dashTime)
        {
            controller.Move(moveDir * dashSpeed * Time.deltaTime);
            

            yield return null;
        }
    }

    //cant use if because it would only dash for one time not a period
    /*
    public void Dash()
    {
        //prepare for add some delay

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float startTimer = Time.time;

            if (Time.time < startTimer + dashTime)
            {
                controller.Move(moveDir * dashSpeed * Time.deltaTime);


            }
        }

    }
    */ 
}
