using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class AntiGravityAttribute : MonoBehaviour, IPotionAttribute
{
    public SpillCollider SpillCollider;
    public Renderer Potion;
    public Material NewPotionMaterial;
    private string _name = "antiGravityAttribute";

    [Header("Air Movement")]
    public float AntiGravityMaxAirMoveSpeed;
    public float AntiGravityAirAccelerationSpeed;
    public float AntiGravityDrag;

    [Header("Jumping")]
    //public bool AntiGravityAllowJumpingWhenSliding;
    public float AntiGravityTimeToMaxJumpApex;
    public float AntiGravityMaxJumpHeight;
    public float AntiGravityMinJumpHeight;
    public float AntiGravityHangTime;
    public float AntiGravityHangtimeGravityDampness;


    // variables to switchback
    [Header("Original Air Movement")]
    public float OriginalMaxAirMoveSpeed;
    public float OriginalAirAccelerationSpeed;
    public float OriginalDrag;

    [Header("Original Jumping")]
    public float OriginalTimeToMaxJumpApex;
    public float OriginalMaxJumpHeight;
    public float OriginalMinJumpHeight;
    public float OriginalHangTime;
    public float OriginalHangtimeGravityDampness;
    public void Equip()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Equiped anti gravity");
        Potion.material = NewPotionMaterial;
        var playerController = player.GetComponent<MainCharacterController>();

        /*
        OriginalMaxAirMoveSpeed  = playerController.MaxAirMoveSpeed;
        OriginalAirAccelerationSpeed = playerController.AirAccelerationSpeed;
        OriginalDrag = playerController.Drag = AntiGravityDrag;
        OriginalTimeToMaxJumpApex = playerController.TimeToMaxJumpApex;
        OriginalMaxJumpHeight = playerController.MaxJumpHeight;
        OriginalMinJumpHeight = playerController.MinJumpHeight;
        OriginalHangTime = playerController.HangTime;
        OriginalHangtimeGravityDampness = playerController.HangtimeGravityDampness;
        */
        // TODO cull unsused jump attributes

        playerController.MaxAirMoveSpeed = AntiGravityMaxAirMoveSpeed;
        playerController.AirAccelerationSpeed = AntiGravityAirAccelerationSpeed;
        playerController.Drag = AntiGravityDrag;
        //playerController.AllowJumpingWhenSliding = AntiGravityAllowJumpingWhenSliding;
        playerController.TimeToMaxJumpApex = AntiGravityTimeToMaxJumpApex;
        playerController.MaxJumpHeight = AntiGravityMaxJumpHeight;
        playerController.MinJumpHeight = AntiGravityMinJumpHeight;
        playerController.HangTime = AntiGravityHangTime;
        playerController.HangtimeGravityDampness = AntiGravityHangtimeGravityDampness;
        //playerController.JumpPreGroundingGraceTime = AntiGravityJumpPreGroundingGraceTime;
        //playerController.JumpPostGroundingGraceTime = AntiGravityJumpPostGroundingGraceTime;
        playerController._lockSpill = false;
        FindObjectOfType<KinematicCharacterController.Examples.PlayerDeathTrigger>()._poolBool = true;
    }

    public void Unequip()
    {

        var player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("UnEquiped anti gravity");
        Potion.material = NewPotionMaterial;
        var playerController = player.GetComponent<MainCharacterController>();

        playerController.MaxAirMoveSpeed = OriginalMaxAirMoveSpeed;
        playerController.AirAccelerationSpeed = OriginalAirAccelerationSpeed;
        playerController.Drag = OriginalDrag;
        playerController.TimeToMaxJumpApex = OriginalTimeToMaxJumpApex;
        playerController.MaxJumpHeight = OriginalMaxJumpHeight;
        playerController.MinJumpHeight = OriginalMinJumpHeight;
        playerController.HangTime = OriginalHangTime;
        playerController.HangtimeGravityDampness = OriginalHangtimeGravityDampness;
        Debug.Log("end  of UnEquiped anti gravity");
    }
    public bool Use()
    {
        foreach(GameObject g in SpillCollider.ObjectsInSplashZone)
        {
            if (g.GetComponent<Floatable>() != null)
            {
                g.GetComponent<Floatable>().Floating = true;
            }
            if (g.GetComponent<OscilateFloat>() != null)
            {
                g.GetComponent<OscilateFloat>().Floating = true;
            }
        }
        return true;
    }

    public string Name { get => _name; }
}
