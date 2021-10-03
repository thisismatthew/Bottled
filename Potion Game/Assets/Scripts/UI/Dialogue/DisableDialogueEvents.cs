using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDialogueEvents : MonoBehaviour
{
    private GameObject[] events;
    private void Awake()
    {
        events = GameObject.FindGameObjectsWithTag("DialogueEvent");
        foreach(GameObject e in events)
        {
            e.active = false;
        }
    }
}
