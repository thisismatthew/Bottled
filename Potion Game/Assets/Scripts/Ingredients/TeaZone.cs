using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeaZone : MonoBehaviour
{
    // Start is called before the first frame update
    public bool TeaSpotComplete = false;
    public DialogueEvent dialogueEvent;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PickUpable" && TeaSpotComplete == false)
        {
            //will need to fix candles beinglit inside the radius
            if (other.transform.GetChild(0).GetComponent<Fillable>()!= false)
            {
                Debug.Log("fillable detected");
                if (other.transform.GetChild(0).GetComponent<Fillable>().Full == true)
                {
                    Debug.Log("Full!");
                    other.GetComponent<Pickupable>().DisablePickup = true;
                    other.GetComponent<Pickupable>().MoveToTarget(transform.position);
                    other.gameObject.AddComponent<LockRotation>();
                    other.GetComponent<LockRotation>().LockMovement = true;
                    TeaSpotComplete = true;

                    WitchDrinksTea();
                }
            }
        }

    }
    private void WitchDrinksTea()
    {
        dialogueEvent.gameObject.SetActive(true);
    }
}
