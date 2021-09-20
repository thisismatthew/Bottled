using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbalTeaAttribute : MonoBehaviour, IPotionAttribute
{
    public Renderer Potion;
    public Material NewLiquidMaterial;
    private string _name = "teaAttribute";

    public void Equip()
    {
        Debug.Log("Equiped tea.");
        Potion.material = NewLiquidMaterial;
    }

    public void Unequip()
    {
        Debug.Log("No tea left.");
    }

    public bool Use()
    {
        return false;
    }

    public string Name { get => _name; }
}
