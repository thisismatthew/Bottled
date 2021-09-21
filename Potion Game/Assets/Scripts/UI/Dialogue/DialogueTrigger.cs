using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using KinematicCharacterController.Examples;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager ui;
    private DialogueEvent currentDialogue = null;
    public CinemachineTargetGroup targetGroup;
    private PlayerInputHandler input;
    private MainCharacterController controller;


    void Start()
    {
        controller = GetComponent<MainCharacterController>();
        input = GetComponent<PlayerInputHandler>();
        ui = DialogueManager.instance;
    }

    void Update()
    {
        if (currentDialogue != null)
        {
            if (!currentDialogue.triggered)
            {
                if (Input.anyKey && !ui.inDialogue && currentDialogue != null)
                {
                    Debug.Log("Initiated Dialogue");
                    
                    for(int i = 0; i<currentDialogue.LookTargets.Count; i++)
                    {
                        targetGroup.m_Targets[i+1].target = currentDialogue.LookTargets[i];
                    }
                    
                    controller.LookTargetOveride = currentDialogue.transform;
                    input.Locked = true;
                    Debug.Log("Dialogue Locked");
                    currentDialogue.active = false;
                    ui.inDialogue = true;
                    ui.CameraChange(true);
                    ui.ClearText();
                    ui.FadeUI(true, .2f, .65f);
                }
            }

            if (currentDialogue.triggered)
            {
                controller.LookTargetOveride = null;
                input.Locked = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DialogueEvent"))
        {
            Debug.Log("Dialogue Triggered");
            currentDialogue = other.GetComponent<DialogueEvent>();
            if (!currentDialogue.triggered)
                ui.currentDialogue = currentDialogue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DialogueEvent"))
        {
            currentDialogue.triggered = true;
            currentDialogue = null;
            ui.currentDialogue = currentDialogue;
        }
    }

}
