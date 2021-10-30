using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using KinematicCharacterController.Examples;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager ui;
    public DialogueEvent currentDialogue = null;
    private PlayerInputHandler input;
    private MainCharacterController controller;
    private bool InsideDialogue = false;
    private bool lockout = false;
    private bool _freeFromDialogueNextFrame=false;

    void Start()
    {
        controller = GetComponent<MainCharacterController>();
        input = GetComponent<PlayerInputHandler>();
        ui = DialogueManager.instance;
    }

    void Update()
    {
        if (_freeFromDialogueNextFrame)
        {
            StopDialog();
            _freeFromDialogueNextFrame = false;
        }

        if (currentDialogue != null)
        {
            if (lockout == false)
            {
                StartDialog();
            }

            if (currentDialogue.triggered && Input.anyKeyDown)
            {
                InsideDialogue = false;
                _freeFromDialogueNextFrame = true;
                if (controller.LookTargetOveride == currentDialogue.transform)
                {
                    controller.LookTargetOveride = null;
                }
                controller.AnimMovementLocked = false;
                input.Locked = false;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("colliding with:" + other.name);
        Debug.Log("inside collider" + InsideDialogue);
        if (other.CompareTag("DialogueEvent") && !InsideDialogue)
        {
            InsideDialogue = true;
            Debug.Log("Dialogue Triggered");
            currentDialogue = other.GetComponent<DialogueEvent>();
            
            if (!currentDialogue.triggered)
            {
                ui.currentDialogue = currentDialogue;
                Debug.Log("Initiated Dialogue");
                controller.AnimMovementLocked = true;
                controller.LookTargetOveride = currentDialogue.transform;
                input.Locked = true;
                Debug.Log("Dialogue Locked");
                currentDialogue.active = false;
                ui.inDialogue = true;
                ui.dialogueCam = currentDialogue.CameraShots[0];
                ui.CameraChange(true);
                ui.ClearText();
                ui.FadeUI(true, .2f, .65f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("DialogueEvent")&&(currentDialogue.triggered))
        {
            InsideDialogue = false;
            currentDialogue = null;
            ui.currentDialogue = currentDialogue;
        }
    }
    public void StartDialog()
    {
        GetComponent<MainCharacterController>().StartDialog();
        lockout = true;
    }

    public void StopDialog()
    {
        GetComponent<MainCharacterController>().StopDialog();
    }
}
