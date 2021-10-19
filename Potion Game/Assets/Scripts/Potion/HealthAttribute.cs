using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttribute : MonoBehaviour, IPotionAttribute
{
    public SpillCollider SpillCollider;
    public Renderer Potion;
    public Material NewLiquidMaterial;
    private string _name = "healthAttribute";

    public void Equip()
    {
        Debug.Log("Equiped final potion.");
        Potion.material = NewLiquidMaterial;
    }

    public void Unequip()
    {

    }

    public bool Use()
    {
        foreach (GameObject g in SpillCollider.ObjectsInSplashZone)
        {
            if (g.GetComponent<Fillable>() != null)
            {
                g.GetComponent<Fillable>().Fill();
            }
        }
        return true;
    }

    public string Name { get => _name; }
}
