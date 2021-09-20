using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class AntiGravityAttribute : MonoBehaviour, IPotionAttribute
{
    public Renderer Potion;
    public Color NewPotionColor;
    private string _name = "antiGravityAttribute";
    public GameObject FirePrefab;
    public bool FireLit = false;

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
        Potion.material.SetColor("_LiquidColour", NewPotionColor);
        var playerController = player.GetComponent<MainCharacterController>();

        // TODO cull unsused jump attributes
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


}

    public void Unequip()
    {
     
    }
    public bool Use()
    {
        //Flamable.equip
        //TODO sprout fire particle affect
        var player = GameObject.FindGameObjectWithTag("Player");
        if (FireLit == false)
        {
            GameObject childobject = Instantiate(FirePrefab, player.transform);//= Instantiate(FirePrefab) as GameObject;
            childobject.transform.localPosition = new Vector3(0, 3, 0);
            player.AddComponent<Flamable>();
            player.GetComponent<Flamable>().Burning = true;
            FireLit = true;
        }
        Debug.Log("Used A anti gravity");
        return true;
    }
    public string Name { get => _name; }
}
