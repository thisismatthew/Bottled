using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class LockPlayer : MonoBehaviour
{
    public static LockPlayer Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void FlipLock()
    {
        PlayerInputHandler.Instance.Locked = !PlayerInputHandler.Instance.Locked;
        FindObjectOfType<MainCharacterController>().AnimMovementLocked = !FindObjectOfType<MainCharacterController>().AnimMovementLocked;
        if (PlayerInputHandler.Instance.Locked) Debug.Log("Locked = " + PlayerInputHandler.Instance.Locked);
    }
}
