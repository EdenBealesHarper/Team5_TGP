﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer_CharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject MeshObject;

    [SerializeField]
    private GameObject Cam;


    [SerializeField]
    public float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.

    public float SpeedModifier = 1f;               //Used to adjust the speed, multiplies current speed by the modifier

    public float JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField]
    private float DoubleJumpMultiplier_Falling  = 6;
    [SerializeField]
    private float DoubleJumpMultiplier_Jumping = 1.5f;

    public float FallMultiplier = 2.5f;

    public float LowJumpMultiplier = 2f;

    [SerializeField]
    private float WeaponTimer = 0.0f;               //How long it takes the player to return to non gun mode.

    private float ActiveWeaponTimer = 0.0f;               //Gun mode timer






    [SerializeField]
    private bool bAirControl = false;                 // Whether or not a player can steer while jumping;

    [SerializeField]
    private LayerMask WhatIsGround;                  // A mask determining what is ground to the character


    private Transform GroundCheck;    // A position marking where to check if the player is grounded.
    const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool bGrounded;            // Whether or not the player is grounded.
    private Transform CeilingCheck;   // A position marking where to check for ceilings
    const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator Anim;            // Reference to the player's animator component.
    private Rigidbody2D rb;
    private bool FacingRight = true;  // For determining which way the player is currently facing.


    public int JumpCount = 0;             // How many times the player has jumped after last touching the floor
    [SerializeField]
    private int MaxJumpCount = 1;   //The maximum amount of times the player can jump

    [SerializeField]
    private AudioClip JumpSFX;

    [SerializeField]
    private bool bIsWeaponActive;


    private bool bJumpHeld = false;
    
    private Springer_AimController AimController;
    private FollowCamera FollowCam;
    private Springer_WeaponManager WeaponManager;

   public float LocalScale;

    private void Awake()
    {
        // Setting up references.
        GroundCheck = transform.Find("FloorCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        Anim = MeshObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        AimController = MeshObject.GetComponent<Springer_AimController>();
        FollowCam = Cam.GetComponent<FollowCamera>();
        WeaponManager = GetComponent<Springer_WeaponManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxJumpCount = 2;  
    }

    // Update is called once per frame
    void Update()
    {
        Anim.ResetTrigger("Jump");
        CalculateFacingDirection();

        WeaponManager.FireWeapon(Input.GetButton("Fire1"));
        WeaponMode(Input.GetButton("Fire1"));
       
        Move(Input.GetAxis("Horizontal"), Input.GetButton("Jump"));
    }

    private void FixedUpdate()
    {
        bGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                bGrounded = true;
                JumpCount = 0;
            }
        }
        Anim.SetBool("Grounded", bGrounded);        

        // Set the vertical animation
        Anim.SetFloat("vSpeed", rb.velocity.y);
    }
    public void Move(float Movement, bool bShouldJump)
    {
        //only control the player if grounded or airControl is turned on
        if (bGrounded || bAirControl)
        {



            // The Speed animator parameter is set to the absolute value of the horizontal input.
            // if (!bIsWeaponActive) Anim.SetFloat("Speed", Mathf.Abs(Movement));
            // else Anim.SetFloat("Speed", Movement);

           if ( bIsWeaponActive && FacingRight) Anim.SetFloat("Speed", Movement);
           else if (bIsWeaponActive && !FacingRight) Anim.SetFloat("Speed", -Movement);
           else Anim.SetFloat("Speed", Mathf.Abs(Movement));


            Anim.SetInteger("Run", (int)(Movement * 10));

            // Move the character
            rb.velocity = new Vector2((Movement * MaxSpeed) * SpeedModifier, rb.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (Movement > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (Movement < 0 && FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }


       
        // If the player should jump...
        if (bShouldJump && ((Anim.GetBool("Grounded") && bGrounded) || JumpCount < MaxJumpCount && !bJumpHeld))
        {

            // Add a vertical force to the player.

            if (bGrounded)
            {
                rb.AddForce(new Vector2(0f, JumpForce));
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(new Vector2(0f, (JumpForce * DoubleJumpMultiplier_Falling)));
            }
            else
            {
                rb.AddForce(new Vector2(0f, JumpForce * DoubleJumpMultiplier_Jumping));
            }

            bGrounded = false;
            Anim.SetBool("Grounded", false);
            Anim.SetTrigger("Jump");

            AudioManager.Instance().PlaySFXPlayer(JumpSFX);

             JumpCount++;
        }
        IsJumpHeld(bShouldJump);
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void Flip()
    {
        if (!bIsWeaponActive)
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    void UpdateWeaponMode()
    {
      
        AimController.ToggleWeaponState(bIsWeaponActive);
        FollowCam.ToggleCameraMode(bIsWeaponActive);
        if (bIsWeaponActive)
        {
            Anim.SetLayerWeight(1, 1.0f);
        }
        else
        {
            Anim.SetLayerWeight(1, 0.0f);
        }
    }
    void WeaponMode(bool bActive)
    {
        if (bActive)
        {
            bIsWeaponActive = true;
           

            ActiveWeaponTimer = WeaponTimer;

        }

        ActiveWeaponTimer -= Time.deltaTime;
        if (ActiveWeaponTimer <= 0)
        {
            bIsWeaponActive = false;
        }

        UpdateWeaponMode();
    }

    void CalculateFacingDirection()
    {
      
        if (bIsWeaponActive)
        {



            Vector3 CharacterPosOnScreen = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 ScreenPos = Input.mousePosition;
            if (ScreenPos.x > (CharacterPosOnScreen.x))
            {
                FacingRight = true;
                // Multiply the player's x local scale by -1.
                Vector3 theScale = transform.localScale;
                theScale.x = LocalScale;
                transform.localScale = theScale;
                WeaponManager.SetDirectionModifier(1);
                
            }
            else if (ScreenPos.x < (CharacterPosOnScreen.x))
            {
                FacingRight = false;
                // Multiply the player's x local scale by -1.
                Vector3 theScale = transform.localScale;
                theScale.x = LocalScale * -1;
                transform.localScale = theScale;
                WeaponManager.SetDirectionModifier(-1);
            }

        }
    }


    void SetSpeedModifier(float NewMod)
    {
        SpeedModifier = NewMod;
    }

    void SetMaxJumpCount(int NewCount)
    {
        MaxJumpCount = NewCount;
    }


    void IsJumpHeld(bool NewInput)
    {
        if (bJumpHeld != NewInput) bJumpHeld = NewInput;
    }
}
