using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
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
    }

    public void Unequip()
    {
        Debug.Log("No fire left... :(");
        FireLit = false;
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
