using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Cinemachine;
using TMPro;
using System.Threading.Tasks;
using KinematicCharacterController.Examples;
using Febucci.UI;

public class DialogueManager : MonoBehaviour
{
    /* This bad boy is in charge of the following things
     * 
     * Locks Player & Input
     * Calls for a camera event
     * Brings up the UI Object
     * Displays Text Blocks (sequentially)
     * Waits for user input (to skip through a block)
     * 
     * When the following tasks are complete:
     * -cameras are done
     * -we've run out of blocks
     * 
     * We hide the UI Object, 
     * Unlock Player Input
     * 
     */

    public static DialogueManager Instance;
    public GameObject DialogueUIContainer;
    public List<Dialogue> AllDialogue;
    private OptionsHelper options;
    private Dialogue current;
    private List<CinemachineVirtualCamera> camShots;
    private int camIndex = 0;
    private Dictionary<string, string> ControllerInputs, KeyboardInputs;
    private TextMeshProUGUI textMeshPro;
    private TextAnimatorPlayer textAnimatorPlayer;
    
    public bool blockComplete = true;

    private void Awake()
    {
        Instance = this;


        ControllerInputs = new Dictionary<string, string>();
        ControllerInputs.Add("[jump]", "A");
        ControllerInputs.Add("[grab]", "X");
        ControllerInputs.Add("[pour]", "B");

        KeyboardInputs = new Dictionary<string, string>();
        KeyboardInputs.Add("[jump]", "space");
        KeyboardInputs.Add("[grab]", "E");
        KeyboardInputs.Add("[pour]", "Q");
    }

    private void Start()
    {
        options = FindObjectOfType<OptionsHelper>();
        textMeshPro = DialogueUIContainer.GetComponentInChildren<TextMeshProUGUI>();
        textAnimatorPlayer = DialogueUIContainer.GetComponentInChildren<TextAnimatorPlayer>();
        textAnimatorPlayer.textAnimator.onEvent += OnEvent;
    }

    public async void BeginDialogueEvent(string _eventName, List<CinemachineVirtualCamera> _shots)
    {
        current = null;
        foreach (Dialogue d in AllDialogue)
        {
            if (d.Name == _eventName)
            {
                current = d;
                camShots = _shots;
            }           
        }
        if (current == null)
        {
            Debug.LogError("No Dialogue with the name" + _eventName + "exists in Dialogue file");
            return; // lil gaurd clause for if theres a null dialogue
        }

        //SETUP
        LockPlayer.Instance.FlipLock(); //this locks the player, but check the input guy as well. 
        DialogueUIContainer.transform.DOScale(0.175f, 1f).SetEase(Ease.InBounce);
        //WAITING
        await RunDialogueSequence(); //execute this and wait for it to finish before moving on. 

        //CLEANUP
        LockPlayer.Instance.FlipLock(); //release the player
        foreach (var c in camShots) c.m_Priority = 0;//clear the priorities on all the camera shots
        camShots = null; //wipe the list of camera shots
        DialogueUIContainer.transform.DOScale(0f, 1f).SetEase(Ease.OutBounce);  
    }

    private string UpdateControlsTags(string text)
    {
        string ret;
        ret = text;
        Dictionary<string, string> dict;
        if (options.GamepadController) dict = ControllerInputs; else dict = KeyboardInputs; //pick the correct dictionary based off of the player options
        foreach(string key in dict.Keys)
        {
            if (text.Contains(key))
                ret = text.Replace(key, dict[key]); // if the string contains a key replace it with the correct value;
        }
        return ret;
    }

    public async Task RunDialogueSequence()
    {
        string displayText;
        //run each block of text -- if a block has the [cam] tag we're going to trigger a camera change. 
        foreach(string s in current.Blocks)
        {
            displayText = UpdateControlsTags(s);
            textAnimatorPlayer.ShowText(displayText);
            await WaitForInput();//wait for the player to click before loading the next text. 
            await Task.Delay(100);
        }
        
    }

    public async Task WaitForInput()
    {  
        while (!Input.GetMouseButtonDown(0))//TODO need to make this work for controllers too
        { 
            await Task.Yield();
        }
        Debug.Log("Input Detected");
    }

    public async Task WaitForInputOrSeconds(float duration)
    {
        float end = Time.time + duration;
        while (Time.time < end && !Input.GetMouseButtonDown(0))
        {
            await Task.Yield();
        }
    }

    private void OnEvent(string message)
    {
        Debug.Log(message);

        //when we hit a cam trigger in the string we'll trigger a camera shot. 
        switch (message)
        {
            case "cam":
                //if this is the last one set all priorities to 0 and get outtie
                if (camIndex > camShots.Count - 1)
                {
                    foreach (var c in camShots) c.m_Priority = 0;
                    return;
                }

                //eeep hopefully this never happens
                if (camShots[camIndex] == null)
                {
                    Debug.LogError("Camera tag called from dialogue but no camera was found... sorry");
                    return;
                }

                //set the camera priority to next shot in our list
                if (camIndex == 0)
                {
                    camShots[0].m_Priority = 20;
                }
                else
                {
                    camShots[camIndex-1].m_Priority = 0;
                    camShots[camIndex].m_Priority = 20;
                }

                

                //prime us for the next cam call
                camIndex++;
                break;
                //we could add another cool tag here like camera shake! or particles, or sound effects!!!!
        }
    }

 
}

