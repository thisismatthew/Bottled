using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

namespace KinematicCharacterController.Examples
{

    public class PlayerInputHandler : MonoBehaviour
    {
        public MainCharacterController Character;
        public GameObject CharacterCamera;
        public bool Locked = false;
        public bool ControllerActive = false;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        private PlayerCharacterInputs NullInput;

        private void Start()
        {
            NullInput = new PlayerCharacterInputs();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.JumpUp = Input.GetKeyUp(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);
            characterInputs.UsePotion = Input.GetKeyDown(KeyCode.E);
            characterInputs.Interact = Input.GetKeyDown(KeyCode.Q);
            characterInputs.SelfDestruct = Input.GetKeyDown(KeyCode.R);
            characterInputs.CameraRotation = CharacterCamera.transform.rotation;
            if (ControllerActive)
            {
                characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Joystick1Button0);
                characterInputs.JumpUp = Input.GetKeyUp(KeyCode.Joystick1Button0);
            }

            // Apply inputs to character
            if (!Locked)
                Character.SetInputs(ref characterInputs);
            else
                Character.SetInputs(ref NullInput);
        }
    }
}