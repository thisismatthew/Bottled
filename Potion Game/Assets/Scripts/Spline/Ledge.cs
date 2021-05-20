using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

[RequireComponent(typeof(Spline))]
public class Ledge : MonoBehaviour
{
    private Spline _spline;
    public Transform PlayerLedgeGrabTarget;
    private MainCharacterController playerController;
    private bool _playerIsClimbing = false;
    public float LedgeGrabDistance = 2f;
    public float LedgeLetGoDistance = 3f;
    private Vector3 _closestPos = new Vector3();
    void Start()
    {
        _spline = GetComponent<Spline>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is in its default movement state (not climbing) this ledge will be aware of the player. 
        if (playerController.CurrentCharacterState == CharacterState.Default && !playerController.JumpingFromClimbing)
        {
            
            //not sure if this will be too taxing on the cpu when there are a bunch of ledges around...
            _closestPos = _spline.GetClosestVertexPosition(PlayerLedgeGrabTarget.position);
           
            
            //if the player is close enough to the closest point on the spline and isn't already climbing this spline...
            //then set the player as climbing this spline
            if (Vector3.Distance(_closestPos, PlayerLedgeGrabTarget.position) < LedgeGrabDistance)
            {
                Debug.Log("climbing started");
                playerController.TransitionToState(CharacterState.Climbing);
                _playerIsClimbing = true;
                playerController.CurrentClimbSpline = _spline;
                
            }
            
        }

        if (_playerIsClimbing)
        {
            //if the player moves outside of the LetGoDistance set climbing back to false
            if (Vector3.Distance(_closestPos, PlayerLedgeGrabTarget.position) > LedgeLetGoDistance)
            {
                Debug.Log("climbing finished");
                _playerIsClimbing = false;
            }
        }
    }
}
