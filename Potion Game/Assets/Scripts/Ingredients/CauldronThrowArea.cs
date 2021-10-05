using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class CauldronThrowArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider player)
    {
        //the player must have the tag Player for the collider to detect them 
        if (player.gameObject.tag == "Player")
        {
            player.GetComponent<MainCharacterController>().CauldronThrowTarget.position = transform.position;
            player.GetComponent<MainCharacterController>().NearCauldron = true;
            player.GetComponent<MainCharacterController>().LookTargetOveride = transform;
        }
    }
    private void OnTriggerExit(Collider player)
    {
        //the player must have the tag Player for the collider to detect them 
        if (player.gameObject.tag == "Player")
        {
            player.GetComponent<MainCharacterController>().NearCauldron = false;
            player.GetComponent<MainCharacterController>().LookTargetOveride = null;
        }
    }
}
