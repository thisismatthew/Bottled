using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public bool inDialogue;
    public Image textBubble;
    public TMP_Animated animatedText;
    

    private int dialogueIndex;
    public bool canExit;
    public bool nextDialogue;
    [HideInInspector]
    public DialogueEvent currentDialogue;
    [Space]

    [Header("Cameras")]
    public CinemachineVirtualCamera dialogueCam;
    public CinemachineTargetGroup camTargetGroup;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && inDialogue)
        {
            if (nextDialogue)
            {
                animatedText.ReadText(currentDialogue.dialogue.conversationBlock[dialogueIndex]);
            }
        }
    }

    public void CameraChange(bool dialogue)
    {
        if (dialogue)
            dialogueCam.m_Priority = 20;
        else
            dialogueCam.m_Priority = 0;
    }

    public void ClearText()
    {
        animatedText.text = string.Empty;
    }

    public void ResetState()
    {
        inDialogue = false;
        canExit = false;
    }

    public void FinishDialogue()
    {
        if (dialogueIndex < currentDialogue.dialogue.conversationBlock.Count - 1)
        {
            dialogueIndex++;
            nextDialogue = true;
        }
        else
        {
            dialogueIndex = 0;
            nextDialogue = false;
            canExit = true;
            Debug.Log("Dialogue Done");
        }
    }
}

