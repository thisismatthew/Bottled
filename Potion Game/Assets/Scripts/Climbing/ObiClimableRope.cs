using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;


namespace Obi
{
    public class ObiClimableRope : MonoBehaviour
    {
        private ObiRope _rope;
        public Transform PlayerRopeGrabPos;
        private MainCharacterController playerController;
        private bool _playerIsClimbing = false;
        private int _closestParticleIndex;
        public float RopeGrabDistance = 2f;
        public float RopeLetGoDistance = 3f;
        private Vector3 _closestPos = new Vector3();
        void Start()
        {
            _rope = GetComponent<ObiRope>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterController>();

        }

        // Update is called once per frame
        void Update()
        {
            //if the player is in its default movement state (not climbing) this rope will be aware of the player. 
            if (playerController.CurrentCharacterState == CharacterState.Default && !playerController.JumpingFromClimbing)
            {
                _closestParticleIndex = _rope.FindClosestRopeParticle(PlayerRopeGrabPos.position);
                //not sure if this will be too taxing on the cpu when there are a bunch of ropes around...
                _closestPos = _rope.GetParticlePosition(_closestParticleIndex);


                //if the player is close enough to the closest point on the rope and isn't already climbing this rope...
                //then set the player as climbing this rope
                if (Vector3.Distance(_closestPos, PlayerRopeGrabPos.position) < RopeGrabDistance)
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                    {
                        Debug.Log("climbing started");
                        playerController.TransitionToState(CharacterState.Climbing);
                        _playerIsClimbing = true;
                        playerController.CurrentClimbRope = _rope;
                    }
                }

            }

            if (_playerIsClimbing)
            {
                //if the player moves outside of the LetGoDistance set climbing back to false
                //TODO remove this without breaking things. 
                if (Vector3.Distance(_closestPos, PlayerRopeGrabPos.position) > RopeLetGoDistance)
                {
                    Debug.Log("climbing finished");
                    _playerIsClimbing = false;
                }
            }
        }
    }
}
