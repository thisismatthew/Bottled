using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KinematicCharacterController.Examples
{
    [RequireComponent(typeof(MainCharacterController))]
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class PlayerDeathTrigger : MonoBehaviour
    {
        
        public float DeadlyFallDistance = 10;
        public float LengthOfTimeDead = 3f;
        public Animator RespawnAnimator;
        public Transform RespawnPoint;
        public Transform SpringWeight;

        private KinematicCharacterMotor Motor;
        private MainCharacterController Controller;
        private float _currentHeight;
        private float _previousHeight;
        [ReadOnly]
        public float _heightFallen;
        private float _timeSlice = 0.1f;
        private float _fallTimer = 0f;
        private float _deadTimer = 0f;
        

        private void Start()
        {
            Controller = GetComponent<MainCharacterController>();
            Motor = GetComponent<KinematicCharacterMotor>();
            _currentHeight = transform.position.y;
            _previousHeight = _currentHeight;


        }
        // Update is called once per frame
        void Update()
        {
            Debug.DrawLine(RespawnPoint.position, Motor.transform.position, Color.red);
           
            _fallTimer += Time.deltaTime;
            if (_fallTimer > _timeSlice)
            {
                _fallTimer = 0;
                _previousHeight = _currentHeight;
                _currentHeight = transform.position.y;
                if (!Motor.GroundingStatus.IsStableOnGround)
                {
                    _heightFallen = _currentHeight - _previousHeight;
                }
            }
            
            

            //if the players height has changed greater than the fall distance in the latest time slice,
            //and they have landed on stable ground. Smash em. 
           if ((_heightFallen< -DeadlyFallDistance)&& Motor.GroundingStatus.IsStableOnGround)
            {
                FindObjectOfType<AudioManager>().Play("Glass_Smash");
                SpringWeight.parent = this.transform;
                Controller.TransitionToState(CharacterState.Dead);
                RespawnAnimator.Play("crossfade_start");
                _heightFallen = 0;
            }

           if (Controller.CurrentCharacterState == CharacterState.Dead)
            {
                _deadTimer += Time.deltaTime;
                if (_deadTimer > LengthOfTimeDead)
                {
                    Motor.SetPosition(RespawnPoint.position);
                    SpringWeight.parent = null;
                    RespawnAnimator.Play("crossfade_end");
                    Controller.TransitionToState(CharacterState.Default);
                    _deadTimer = 0;
                }
            }
            
        }
    }
}

