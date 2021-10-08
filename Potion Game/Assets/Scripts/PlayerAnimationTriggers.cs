using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public void TriggerDefault()
    {
        GetComponentInParent<KinematicCharacterController.Examples.MainCharacterController>().ReturnToDefaultState();
    }
}
