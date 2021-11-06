using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HerbalTeaAttribute : MonoBehaviour, IPotionAttribute
{
    public SpillCollider SpillCollider;
    public Renderer Potion;
    public Material NewLiquidMaterial;
    private string _name = "teaAttribute";
    private VisualEffect TeaLeaves;

    public void Start()
    {
        TeaLeaves = Potion.gameObject.transform.GetChild(0).GetComponent<VisualEffect>();
    }

    public void Update()
    {
        if (TeaLeaves.enabled == true)
        {
            Vector3 copyNormal = Potion.GetComponent<SpringSystem>().NormalFacingPublic;
            TeaLeaves.SetVector3("_PlaneCollider", copyNormal);
        }
    }

    public void Equip()
    {
        Debug.Log("Equiped tea.");
        Potion.material = NewLiquidMaterial;
        TeaLeaves.enabled = true;
        TeaLeaves.Play();
        FindObjectOfType<ReflectionProbe>().GetComponent<Renderer>().material.SetFloat("_Occlusion", 2);
    }

    public void Unequip()
    {
        Potion.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("No tea left.");
        TeaLeaves.enabled = false;
        FindObjectOfType<ReflectionProbe>().GetComponent<Renderer>().material.SetFloat("_Occlusion", 1);
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
