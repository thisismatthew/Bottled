using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class JumpZone : MonoBehaviour
{
    public Transform LandingTarget;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player") player.GetComponent<MainCharacterController>().LandingTarget = LandingTarget;

    }
}
