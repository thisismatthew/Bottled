using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
    public Light FireLight;
    public Transform FlameParticlePosition;
    public Renderer Potion;
    public Material NewLiquidMaterial;
    private string _name = "fireAttribute";
    public GameObject FirePrefab;
    public bool FireLit = false;

    public void Equip()
    {
        Debug.Log("Equiped fire!!!!");
        Potion.material = NewLiquidMaterial;
        FireLight.enabled = true;
    }

    public void Unequip()
    {
        Debug.Log("UnEquiped fire ");
        FireLit = false;
        FireLight.enabled = false;
        FirePrefab.SetActive(false);
    }

    public bool Use()
    {

        GameObject childobject = Instantiate(FirePrefab, FlameParticlePosition);//= Instantiate(FirePrefab) as GameObject;
        childobject.transform.localPosition = Vector3.zero;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (FireLit == false)
        {
            player.AddComponent<Flamable>();
            player.GetComponent<Flamable>().Burning = true;
            FireLit = true;
        }   
        Debug.Log("Used A fire");
        return true;
    }

    public string Name { get => _name; }
}
