using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public CanvasGroup canvasGroup;
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

        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (inDialogue)
            {
                if (canExit)
                {
                    CameraChange(false);
                    FadeUI(false, .2f, 0);
                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(.8f);
                    s.AppendCallback(() => ResetState());
                }

                if (nextDialogue)
                {
                    Debug.Log("next Input Requested");
                    animatedText.ReadText(currentDialogue.dialogue.conversationBlock[dialogueIndex]);
                    nextDialogue = false;
                }
                else
                {
                    StopCoroutine(animatedText.readCoroutine);
                    animatedText.SkipText(currentDialogue.dialogue.conversationBlock[dialogueIndex]);
                    //nextDialogue = true;
                }
            }
        }
    }

    public void FadeUI(bool show, float time, float delay)
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(delay);
        s.Append(canvasGroup.DOFade(show ? 1 : 0, time));
        if (show)
        {
            dialogueIndex = 0;
            s.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
            s.AppendCallback(() => animatedText.ReadText(currentDialogue.dialogue.conversationBlock[0]));
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
            currentDialogue.triggered = true;
            dialogueIndex = 0;
            nextDialogue = false;
            canExit = true;
            Debug.Log("Dialogue Done");
        }
    }
}

