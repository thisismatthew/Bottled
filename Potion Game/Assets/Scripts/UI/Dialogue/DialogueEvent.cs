using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;


public class DialogueEvent : MonoBehaviour
{
    public string DialogueName;
    public bool triggered = false;
    public List<CinemachineVirtualCamera> CameraShots;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            DialogueManager.Instance.BeginDialogueEvent(DialogueName, CameraShots);
            triggered = true;
        }
    }
}
