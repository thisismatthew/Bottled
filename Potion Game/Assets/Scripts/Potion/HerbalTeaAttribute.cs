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
            if (g.GetComponentInChildren<Fillable>() != null)
            {
                Debug.Log("we got one!: " + g.name);
                g.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                g.GetComponent<Pickupable>().FillVolume();

                //Temporary testing placement, please find better spot
            }
        }
        return true;
    }

    public string Name { get => _name; }
}
