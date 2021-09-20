using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttribute : MonoBehaviour, IPotionAttribute
{
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
    }

    public bool Use()
    {
        //Flamable.equip
        //TODO sprout fire particle affect
        var player = GameObject.FindGameObjectWithTag("Player");
        if (FireLit == false)
        {
            GameObject childobject = Instantiate(FirePrefab, player.transform);//= Instantiate(FirePrefab) as GameObject;
            childobject.transform.localPosition = new Vector3(0, 3, 0);
            player.AddComponent<Flamable>();
            player.GetComponent<Flamable>().Burning = true;
            FireLit = true;
        }   
        Debug.Log("Used A fire");
        return true;
    }

    public string Name { get => _name; }
}
