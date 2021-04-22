using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPotionAttribute
{
    string Name { get; set; }
    bool Use();
    void Equip();
    void Unequip();

}
