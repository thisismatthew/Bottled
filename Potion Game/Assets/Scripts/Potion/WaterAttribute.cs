using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttribute : MonoBehaviour, IPotionAttribute
{
    private string _name = "waterAttribute";

    public void Equip()
    {
        Debug.Log("Equiped Water!!!!");
    }

    public void Unequip()
    {
        Debug.Log("No water left... :(");
    }

    public bool Use()
    {
        Debug.Log("Used A Water");
        return true;
    }

    public string Name { get => _name; }
}
