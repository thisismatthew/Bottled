using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionEnd : MonoBehaviour
{
    public GameObject FinalDialogueEvent;
    public GameObject CreditsTimeline;

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<HealthAttribute>())
            {
                FinalDialogueEvent.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (FinalDialogueEvent.activeInHierarchy)
        {
            if (FinalDialogueEvent.GetComponent<DialogueEvent>().triggered && FindObjectOfType<DialogueManager>().inDialogue == false)
            {
                CreditsTimeline.SetActive(true);
            }
        }
    }
}
