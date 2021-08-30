using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueEvent : MonoBehaviour
{
    public DialogueData dialogue;
    private TMP_Animated animatedText;
    public bool active = false;
    public bool triggered = false; 

    // Start is called before the first frame update
    void Start()
    {
        animatedText = DialogueManager.instance.animatedText;
    }

}
