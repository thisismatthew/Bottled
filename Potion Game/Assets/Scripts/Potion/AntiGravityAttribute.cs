using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class AntiGravityAttribute : MonoBehaviour, IPotionAttribute
{
    public Renderer Potion;
    public Material NewPotionMaterial;
    private string _name = "antiGravityAttribute";

    [Header("Air Movement")]
    public float AntiGravityMaxAirMoveSpeed;
    public float AntiGravityAirAccelerationSpeed;
    public float AntiGravityDrag;

    [Header("Jumping")]
    public bool AntiGravityAllowJumpingWhenSliding;
    public float AntiGravityTimeToMaxJumpApex;
    public float AntiGravityMaxJumpHeight;
    public float AntiGravityMinJumpHeight;
    public float AntiGravityHangTime;
    public float AntiGravityHangtimeGravityDampness;
    public float AntiGravityJumpPreGroundingGraceTime;
    public float AntiGravityJumpPostGroundingGraceTime;
    public void Equip()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Equiped anti gravity");
        Potion.material = NewPotionMaterial;
        var playerController = player.GetComponent<MainCharacterController>();

        // TODO cull unsused jump attributes
       /* 
        playerController.MaxAirMoveSpeed = AntiGravityMaxAirMoveSpeed;
        playerController.AirAccelerationSpeed = AntiGravityAirAccelerationSpeed;
        playerController.Drag = AntiGravityDrag;
        playerController.AllowJumpingWhenSliding = AntiGravityAllowJumpingWhenSliding;
        playerController.TimeToMaxJumpApex = AntiGravityTimeToMaxJumpApex;
        playerController.MaxJumpHeight = AntiGravityMaxJumpHeight;
        playerController.MinJumpHeight = AntiGravityMinJumpHeight;
        playerController.HangTime = AntiGravityHangTime;
        playerController.HangtimeGravityDampness = AntiGravityHangtimeGravityDampness;
        playerController.JumpPreGroundingGraceTime = AntiGravityJumpPreGroundingGraceTime;
        playerController.JumpPostGroundingGraceTime = AntiGravityJumpPostGroundingGraceTime;
       */
    }

    public void Unequip()
    {
     
    }
    public bool Use()
    {
        Debug.Log("Used A anti gravity");
        return true;
    }
    public string Name { get => _name; }
}
