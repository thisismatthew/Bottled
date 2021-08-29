using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using KinematicCharacterController.Examples;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager ui;
    private DialogueEvent currentDialogue;
    public CinemachineTargetGroup targetGroup;
    private PlayerInputHandler input;


    void Start()
    {
        input = FindObjectOfType<PlayerInputHandler>();
        ui = DialogueManager.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !ui.inDialogue && currentDialogue != null)
        {
            Debug.Log("Initiated Dialogue");
            targetGroup.m_Targets[1].target = currentDialogue.transform;
            input.Locked = true;
            currentDialogue.active = false;
            ui.inDialogue = true;
            ui.CameraChange(true);
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
