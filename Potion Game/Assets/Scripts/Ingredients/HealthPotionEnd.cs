using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionEnd : MonoBehaviour
{
    public GameObject FinalDialogueEvent;
    public GameObject CreditsTimeline;
    public GameObject Credits;
    

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Potion>().Attributes.Count > 0)
            {
                //Debug.Log(other.GetComponent<Potion>().Attributes[0].Name);
                if (other.GetComponent<Potion>().Attributes[0].Name == "healthAttribute")
                {
                    FinalDialogueEvent.SetActive(true);
                }
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
                Credits.SetActive(true);
            }
        }
    }
}
