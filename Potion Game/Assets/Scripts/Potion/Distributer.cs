using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Distributer : MonoBehaviour
{
    // This Class distributes attributes when the player walks into a collider that this is attatched to. 
    
    public IPotionAttribute AttibuteOne;
    public int SetChargesToDeliver;
    private Potion newPotion;
    private List<IPotionAttribute> SetAttributesToDistribute;

    private void Start()
    {
        SetAttributesToDistribute = new List<IPotionAttribute>();
        /*
        foreach(IPotionAttribute a in GetComponents<IPotionAttribute>())
        {
            SetAttributesToDistribute.Add(a);
        }
        */
    }
    public void FillDistributor(List<IPotionAttribute> PotionFill)
    {
        SetAttributesToDistribute = new List<IPotionAttribute>(PotionFill);
    }

    private void OnTriggerEnter(Collider player)
    {
        //the player must have the tag Player for the collider to detect them 
        if (player.gameObject.tag == "Player")
        {
            //assign it our attributes
            Debug.Log("distribute starting");
            player.GetComponent<Potion>().EquipPotion(SetAttributesToDistribute);
            player.GetComponent<Potion>().Charges = SetChargesToDeliver;
            //TODO probably don't need to search for the component twice but im too lazy to check if I can get a ref rather than a copy. 
            player.GetComponent<Potion>().DebugAttributes();
        }
    }
}
