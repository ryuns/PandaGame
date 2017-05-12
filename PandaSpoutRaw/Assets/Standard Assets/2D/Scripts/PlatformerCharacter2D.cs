using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_SmashForce = -400f;                  // Amount of force added when the player smashes.
        [SerializeField] private float m_SmashForwardForce = 400f;                  // Amount of force added when the player smashes.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private bool Use_Doomguy = false;

        [SerializeField]
        string landingSound = "LandingFootsteps";

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded         // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

		Transform playerGraphics;

        AudioManager audioManager;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            if (Use_Doomguy == true)
            {
                m_Anim = GetComponentInChildren<Animator>();
            }
            else
            {
                m_Anim = GetComponent<Animator>();
            }
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			playerGraphics = transform.FindChild ("Graphics");
			if (playerGraphics == null)
			{
				Debug.LogError ("Let's freak out");
			}
        }

        private void Start()
        {
            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("ERROR: No audiomanager in scene");
            }
        }

        private void FixedUpdate()
        {


        }


        public void Move(float move, float vertical, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);
                vertical = (crouch ? vertical * m_CrouchSpeed : vertical);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed, m_Rigidbody2D.velocity.y);
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, vertical * m_MaxSpeed);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                    if (Use_Doomguy == true)
                    {
                        ArmRotation.rotationOffset = 0;
                    }
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                    if (Use_Doomguy == true)
                    {
                        ArmRotation.rotationOffset = 180;
                    }
                }
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = playerGraphics.localScale;
            theScale.x *= -1;
            playerGraphics.localScale = theScale;
        }
    }
}
