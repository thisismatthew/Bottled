using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
    private string _name = "fireAttribute";

    public void Equip()
    {
        Debug.Log("Equiped fire!!!!");
        //TODO set colour
    }

    public void Unequip()
    {
        Debug.Log("No fire left... :(");
    }

    public bool Use()
    {
        //Flamable.equip
        //TODO sprout fire particle affect
        gameObject.AddComponent<Flamable>();
        gameObject.GetComponent<Flamable>().Burning = true;
        Debug.Log("Used A fire");
        return true;
    }

    public string Name { get => _name; }
}
