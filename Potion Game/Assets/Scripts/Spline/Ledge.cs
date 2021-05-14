using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

[RequireComponent(typeof(Spline))]
public class Ledge : MonoBehaviour
{
    private Spline _spline;
    private Transform _player;
    private bool _playerIsCimbing = false;
    public float LedgeGrabDistance = 2f;
    public float LedgeLetGoDistance = 3f;
    private Vector3 _closestPos = new Vector3();
    void Start()
    {
        _spline = GetComponent<Spline>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerIsCimbing == false)
        {
            //not sure if this will be too taxing on the cpu when there are a bunch of ledges around...
            _closestPos = _spline.GetClosestVertexPosition(_player.position);

            //if the player is close enough to the closest point on the spline and isn't already climbing this spline...
            //then set the player as climbing this spline
            if (Vector3.Distance(_closestPos, _player.position) < LedgeGrabDistance)
            {

                Debug.Log("climbing started");
                _player.GetComponent<MainCharacterController>().TransitionToState(CharacterState.Climbing);
                
                _player.GetComponent<MainCharacterController>().CurrentClimbSpline = _spline;
                _playerIsCimbing = true;
            }
        }

        if (_playerIsCimbing)
        {
            //if the player moves outside of the LetGoDistance set climbing back to false
            if (Vector3.Distance(_closestPos, _player.position) > LedgeLetGoDistance)
            {
                Debug.Log("climbing finished");
                _playerIsCimbing = false;
            }
        }
    }
}
