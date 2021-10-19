using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbalTeaAttribute : MonoBehaviour, IPotionAttribute
{
    public SpillCollider SpillCollider;
    public Renderer Potion;
    public Material NewLiquidMaterial;
    private string _name = "teaAttribute";

    public void Equip()
    {
        Debug.Log("Equiped tea.");
        Potion.material = NewLiquidMaterial;
        Potion.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Unequip()
    {
        Potion.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("No tea left.");
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
