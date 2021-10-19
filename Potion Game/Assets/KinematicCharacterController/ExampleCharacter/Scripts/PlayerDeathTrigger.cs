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
        public Transform DeathBlobSpawn;
        public float CheckpointResetFrequency = 1;
        public ParticleSystem DeathBlobs;

        public Animator anim;

        private KinematicCharacterMotor Motor;
        private MainCharacterController Controller;
        private float _currentHeight;
        private float _previousHeight;
        [ReadOnly]
        public float _heightFallen;
        private float _timeSlice = 0.1f;
        private float _fallTimer = 0f;
        private float _deadTimer = 0f;
        private float _checkpointTimer = 0f;
        

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
            if (Controller.Motor.GroundingStatus.IsStableOnGround)
            {
                _checkpointTimer += Time.deltaTime;
                if (_checkpointTimer> CheckpointResetFrequency)
                    RespawnPoint.position = Controller.gameObject.transform.position;
            }
            else
            {
                _checkpointTimer = 0;
            }
                
           // Debug.DrawLine(RespawnPoint.position, Motor.transform.position, Color.red);
           
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
                _checkpointTimer = 0;
                SmashCharacter();
           }

           if (_heightFallen < -DeadlyFallDistance+0.5f)
           {
                anim.SetTrigger("DeathHeight");
           }

            if (Controller.CurrentCharacterState == CharacterState.Dead)
            {
                _checkpointTimer = 0;
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

        public void SmashCharacter()
        {
            Blob();
            FindObjectOfType<AudioManager>().Play("Glass_Smash");
            SpringWeight.parent = this.transform;
            Controller.TransitionToState(CharacterState.Dead);
            RespawnAnimator.Play("crossfade_start");
            _heightFallen = 0;
        }

        public void Blob()
        {
            DeathBlobs.transform.position = DeathBlobSpawn.position;
            DeathBlobs.Play();
        }
    }
}

