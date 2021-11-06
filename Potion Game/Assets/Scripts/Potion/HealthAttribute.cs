using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttribute : MonoBehaviour, IPotionAttribute
{
    public SpillCollider SpillCollider;
    public Renderer Potion;
    public Material NewLiquidMaterial;
    public GameObject DialogueEvent;
    private string _name = "healthAttribute";

    public void Equip()
    {
        Debug.Log("Equiped final potion.");
        Potion.material = NewLiquidMaterial;
        FindObjectOfType<ReflectionProbe>().GetComponent<Renderer>().material.SetFloat("_Occlusion", 2);
        FindObjectOfType<KinematicCharacterController.Examples.MainCharacterController>()._lockSpill = false;
        FindObjectOfType<KinematicCharacterController.Examples.PlayerDeathTrigger>()._poolBool = true;
    }

    public void Unequip()
    {
        FindObjectOfType<ReflectionProbe>().GetComponent<Renderer>().material.SetFloat("_Occlusion", 1);
    }

    public bool Use()
    {
        DialogueEvent.SetActive(true);
        return true;
    }

    public string Name { get => _name; }
}
