using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Cinemachine;

namespace KinematicCharacterController.Examples
{

    public class PlayerInputHandler : MonoBehaviour
    {
        public MainCharacterController Character;
        public GameObject CharacterCamera;
        public CinemachineFreeLook FreeLookVirtualCamera;
        public bool Locked = false;
        public bool EnableSelfDestructButton = false;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        private PlayerCharacterInputs NullInput;
        private OptionsHelper options;


        private void Awake()
        {
            if (FindObjectOfType<OptionsHelper>() == null)
            {
                Debug.LogError("No Player Options Detected: Adding One To Player Input Handler.");
                gameObject.AddComponent<OptionsHelper>();
            }
        }

        private void Start()
        {
            NullInput = new PlayerCharacterInputs();
            Cursor.lockState = CursorLockMode.Locked;
            options = FindObjectOfType<OptionsHelper>();
           
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
            characterInputs.UsePotion = Input.GetKeyDown(KeyCode.Q);
            characterInputs.Interact = Input.GetKeyDown(KeyCode.E);

            if(EnableSelfDestructButton)
                characterInputs.SelfDestruct = Input.GetKeyDown(KeyCode.R);
            
            if (options.GamepadController)
            {
                characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Joystick1Button0);
                characterInputs.JumpUp = Input.GetKeyUp(KeyCode.Joystick1Button0);
                characterInputs.UsePotion = Input.GetKeyDown(KeyCode.Joystick1Button1);
                characterInputs.Interact = Input.GetKeyDown(KeyCode.Joystick1Button2);
                FreeLookVirtualCamera.m_XAxis.m_InputAxisName = "Joystick X";
                FreeLookVirtualCamera.m_XAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
                FreeLookVirtualCamera.m_XAxis.m_MaxSpeed = 50;
                FreeLookVirtualCamera.m_YAxis.m_InputAxisName = "Joystick Y";
                FreeLookVirtualCamera.m_YAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
                FreeLookVirtualCamera.m_YAxis.m_MaxSpeed = 0.5f;
                FreeLookVirtualCamera.m_YAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
                FreeLookVirtualCamera.m_YAxis.m_InvertInput = false;
            }

            characterInputs.CameraRotation = CharacterCamera.transform.rotation;
            // Apply inputs to character
            if (!Locked)
                Character.SetInputs(ref characterInputs);
            else
                Character.SetInputs(ref NullInput);
        }
    }
}