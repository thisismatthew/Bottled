using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
    public Renderer Potion;
    public Color NewPotionColor;
    private string _name = "fireAttribute";
    public GameObject FirePrefab;

    public void Equip()
    {
        Debug.Log("Equiped fire!!!!");
        Potion.material.SetColor("_LiquidColour", NewPotionColor);
    }

    public void Unequip()
    {
        Debug.Log("No fire left... :(");
    }

    public bool Use()
    {
        GameObject childobject = Instantiate(FirePrefab) as GameObject;
        childobject.transform.parent = gameObject.transform;
        //FirePrefab.transform.parent = this.transform;
        //Flamable.equip
        //TODO sprout fire particle affect
        gameObject.AddComponent<Flamable>();
        gameObject.GetComponent<Flamable>().Burning = true;
        Debug.Log("Used A fire");
        return true;
    }

    public string Name { get => _name; }
}
