using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    public TextAsset SceneDialogue;
    //private DialogueController dc;
    private RawDialogueList dialogueList = new RawDialogueList();

    private List<Dialogue> DialogueEventData;


    [System.Serializable]
    public class RawDialogueData
    {
        public string Name;
        public string Desc;
        //This sucks but I'm too tired to think of a way to import a list! ¯\_(ツ)_/¯ - Matt
        public string
            Block_0,
            Block_1,
            Block_2,
            Block_3,
            Block_4,
            Block_5,
            Block_6,
            Block_7,
            Block_8,
            Block_9;
    }

    [System.Serializable]
    public class RawDialogueList
    {
        public List<RawDialogueData> Dialogue;
        public RawDialogueList()
        {
            Dialogue = new List<RawDialogueData>();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        DialogueEventData = new List<Dialogue>();
        //dc = FindObjectOfType<DialogueController>();
        dialogueList = JsonUtility.FromJson<RawDialogueList>(SceneDialogue.text);
        
        Dialogue new_D;

        foreach (RawDialogueData raw in dialogueList.Dialogue)
        {
            new_D = new Dialogue(raw.Name, raw.Desc);
            if (raw.Block_0 != "") new_D.Blocks.Add(raw.Block_0);
            if (raw.Block_1 != "") new_D.Blocks.Add(raw.Block_1);
            if (raw.Block_2 != "") new_D.Blocks.Add(raw.Block_2);
            if (raw.Block_3 != "") new_D.Blocks.Add(raw.Block_3);
            if (raw.Block_4 != "") new_D.Blocks.Add(raw.Block_4);
            if (raw.Block_5 != "") new_D.Blocks.Add(raw.Block_5);
            if (raw.Block_6 != "") new_D.Blocks.Add(raw.Block_6);
            if (raw.Block_7 != "") new_D.Blocks.Add(raw.Block_7);
            if (raw.Block_8 != "") new_D.Blocks.Add(raw.Block_8);
            if (raw.Block_9 != "") new_D.Blocks.Add(raw.Block_9);
            DialogueEventData.Add(new_D);
        }


        
        // At this point we have a nice single object that holds all our dialogue each with a name that acts as its index. 
        // Time to send it to the dialogue manager. 
    }
    private void Start()
    {
        DialogueManager.Instance.AllDialogue = DialogueEventData;
    }
}
