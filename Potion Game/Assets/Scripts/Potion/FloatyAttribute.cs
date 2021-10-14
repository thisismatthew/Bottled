using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyAttribute : MonoBehaviour, IPotionAttribute
{
    public Renderer Potion;
    public Color NewPotionColor;
    private string _name = "floatyAttribute";
    public GameObject FloatPrefab;
    public bool particles = false;
    public bool FloatItems = false;

    public void Equip()
    {
        Debug.Log("Equiped floatiness!!!!");
        Potion.material.SetColor("_LiquidColour", NewPotionColor);
    }

    public void Unequip()
    {
        Debug.Log("No floatiness left... :(");
    }

    public bool Use()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (particles == false)
        {
            GameObject childobject = Instantiate(FloatPrefab, player.transform);
            particles = true;
        }
        Debug.Log("Used A floatiness");
        FloatItems = true;
        return true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Floaty collision 1");
        if (collider.gameObject.GetComponent<Floatable>() != null)
        {
            Debug.Log("Floaty collision 2");
                var floatableObject = collider.gameObject;
                if (floatableObject.GetComponent<Floatable>().Floating == false)
                {
                Debug.Log("Floaty not floating");
                floatableObject.GetComponent<Floatable>().Floating = true;
                Debug.Log("Floaty should float");
            }
        }
    }

    public string Name { get => _name; }
}

