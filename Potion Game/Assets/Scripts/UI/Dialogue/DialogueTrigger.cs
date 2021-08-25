using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager ui;
    private DialogueEvent currentDialogue;

    void Start()
    {
        ui = DialogueManager.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !ui.inDialogue && currentDialogue != null)
        {
            currentDialogue.active = false;
            ui.inDialogue = true;
            ui.ClearText();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DialogueEvent"))
        {
            Debug.Log("Dialogue Triggered");
            currentDialogue = other.GetComponent<DialogueEvent>();
            ui.currentDialogue = currentDialogue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DialogueEvent"))
        {
            currentDialogue = null;
            ui.currentDialogue = currentDialogue;
        }
    }

}
