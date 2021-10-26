using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class LockPlayer : MonoBehaviour
{
    public void FlipLock()
    {
        FindObjectOfType<PlayerInputHandler>().Locked = !FindObjectOfType<PlayerInputHandler>().Locked;
        FindObjectOfType<MainCharacterController>().AnimMovementLocked = !FindObjectOfType<MainCharacterController>().AnimMovementLocked;
    }
}
