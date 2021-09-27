using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualSpot : MonoBehaviour
{
    public bool RitualSpotComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Candle")
        {
            other.GetComponentInParent<Pickupable>().MoveToTarget(transform.position);
            other.GetComponentInParent<Pickupable>().enabled = false;
            RitualSpotComplete = true;
        }
    }
}
