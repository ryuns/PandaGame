using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        //(bool) Will return if the Fizzyo button is pressed or not.
        bool buttonPressed = Fizzyo.FizzyoDevice.Instance().ButtonDown();

        //Alternatively, you can get the button state directly using:
        bool buttonPresed = Input.GetButtonDown("Fire1");

        //(float) returns breath strength from (-1 – 1) with 0 being not breathing,
        // > 0.7 blowing or breathing out hard and< -0.5 breathing in hard
        float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();

        //Alternatively, you can get the axis data directly using:

        pressure = Input.GetAxis("Horizontal");


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            // Pass all parameters to the character control script.
            m_Character.Move(h, v, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
