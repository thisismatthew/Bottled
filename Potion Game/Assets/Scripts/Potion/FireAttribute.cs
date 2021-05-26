using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
    private string _name = "fireAttribute";

    public void Equip()
    {
        Debug.Log("Equiped fire!!!!");
        //set colour
    }

    public void Unequip()
    {
        Debug.Log("No fire left... :(");
    }

    public bool Use()
    {
        Debug.Log("Used A fire");
        return true;
    }

    public string Name { get => _name; }
}
