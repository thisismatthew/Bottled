using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAwakeDialogue : MonoBehaviour
{
    public DialogueEvent ChainFrom, ChainTo;
    private bool swapped = false;

    //depreciatied 
/*    private void Update()
    {
        if (ChainFrom.triggered && !swapped)
        {
            swapped = true;
            FindObjectOfType<DialogueTrigger>().currentDialogue = null;
            FindObjectOfType<DialogueTrigger>().InsideDialogue = false;
            ChainTo.gameObject.SetActive(true);
        }
    }*/
}
