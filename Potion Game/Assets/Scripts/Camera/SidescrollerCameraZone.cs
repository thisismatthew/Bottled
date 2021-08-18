using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using Cinemachine;

public class SidescrollerCameraZone : MonoBehaviour
{
    public CinemachineVirtualCamera ZoneCamera;
    
    private void Start()
    {
        ZoneCamera.Priority = 0;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            ZoneCamera.Priority = 11;
           
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            ZoneCamera.Priority = 0;
        }
    }
}
