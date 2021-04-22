using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int Charges { get; set; }
    public List<IPotionAttribute>[] Attributes;

    public void UsePotion()
    {
        foreach (IPotionAttribute a in Attributes)
        {
            a.Use();
        }
    }

    public void EquipPotion(List<IPotionAttribute>[] _newAttributes)
    {
        //unequip all attributes
        foreach (IPotionAttribute a in Attributes)
        {
            a.Unequip();
        }
        Attributes = _newAttributes;
        foreach (IPotionAttribute a in Attributes)
        {
            a.Equip();
        }
    }

    public void DebugAttributes()
    {
        Debug.Log("\nCurrent Potion Attributes:");
        foreach (IPotionAttribute a in Attributes)
        {
            Debug.Log("\n");
            Debug.Log(a.Name);
        }
    }
}
