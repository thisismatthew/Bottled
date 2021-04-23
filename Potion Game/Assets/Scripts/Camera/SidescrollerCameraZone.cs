using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class SidescrollerCameraZone : MonoBehaviour
{
    public Camera SideScrollerCamera;
    public Camera MainCamera;

    private void Start()
    {
        SideScrollerCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider player)
    {

        if (player.gameObject.tag == "Player")
        {
            SideScrollerCamera.enabled = true;
            MainCamera.enabled = false;
            //TODO change user input so that left and right are correct
            
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            MainCamera.enabled = true;
            SideScrollerCamera.enabled = false;
            //TODO change user input so that left and right are back to normal

        }
    }
}
