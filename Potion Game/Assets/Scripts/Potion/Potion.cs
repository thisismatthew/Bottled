using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{ 
    private int _charges; 
    
    public List<IPotionAttribute> Attributes;
    public void UsePotion()
    {
        //do we have any charges of potion left
        if (_charges > 0)
        {
            //ok we do, lets reduce one and call the use method on all attributes
            _charges--;
            foreach (IPotionAttribute a in Attributes) { a.Use(); }
            //if that was our last charge call unequip on all attributes. 
            if (_charges <= 0)
            {
                foreach (IPotionAttribute a in Attributes) { a.Unequip(); }
            }
        }
    }

    public void EquipPotion(List<IPotionAttribute> _newAttributes)
    {
        //unequip all attributes if we haven't already. 
        if (_charges > 0)
        {
            foreach (IPotionAttribute a in Attributes) { a.Unequip(); }
        }
        
        //bring in the new ones
        Attributes = _newAttributes;
        foreach (IPotionAttribute a in Attributes) { a.Equip(); }
    }

    //just a debug method for calling the ID names on the attributes. 
    public void DebugAttributes()
    {
        Debug.Log("\nCurrent Potion Attributes:");
        foreach (IPotionAttribute a in Attributes)
        {
            Debug.Log("\n");
            Debug.Log(a.Name);
        }
    }
    public int Charges { get => _charges; set => _charges = value; }
}

