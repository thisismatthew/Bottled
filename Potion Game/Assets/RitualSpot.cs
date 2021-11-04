using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualSpot : MonoBehaviour
{
    public bool RitualSpotComplete = false;
    public Material FlameColourChange;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Candle" && RitualSpotComplete == false)
        {
            //will need to fix candles beinglit inside the radius
            if (other.GetComponentInParent<Flamable>().Burning)
            {
                other.GetComponentInParent<Pickupable>().DisablePickup = true;
                other.GetComponentInParent<Flamable>().FlameParticlePosition.GetComponentInChildren<Renderer>().material = FlameColourChange;
                other.GetComponentInParent<LockRotation>().LockMovement = true;
                other.GetComponentInParent<Pickupable>().MoveToTarget(transform.position);
                RitualSpotComplete = true;
            }
        }
            
    }
}
