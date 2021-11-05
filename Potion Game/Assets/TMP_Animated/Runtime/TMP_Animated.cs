using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{

    [System.Serializable] public class TextRevealEvent : UnityEvent<char> { }

    [System.Serializable] public class DialogueEvent : UnityEvent { }


    public class TMP_Animated : TextMeshProUGUI
    {

        [SerializeField] private float speed = 10;

        public TextRevealEvent onTextReveal;
        public DialogueEvent onDialogueFinish;
        public Coroutine readCoroutine;
        private Dictionary<string, string> ControllerInputs, KeyboardInputs;
        public bool ReadPaused = false;

        private void LoadInputDictionaries()
        {
            ControllerInputs = new Dictionary<string, string>();
            KeyboardInputs = new Dictionary<string, string>();
            ControllerInputs.Add("jump", "A");
            ControllerInputs.Add("grab", "X");
            ControllerInputs.Add("pour", "B");

            KeyboardInputs.Add("jump", "space");
            KeyboardInputs.Add("grab", "E");
            KeyboardInputs.Add("pour", "Q");
        }


        public void ReadText(string newText)
        {
            LoadInputDictionaries();
            text = string.Empty;
            // split the whole text into parts based off the <> tags 
            // even numbers in the array are text, odd numbers are tags
            string[] subTexts = newText.Split('<', '>');

            // textmeshpro still needs to parse its built-in tags, so we only include noncustom tags
            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                    displayText += subTexts[i];
                else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                    displayText += $"<{subTexts[i]}>";
            }
            // check to see if a tag is our own
            bool isCustomTag(string tag)
            {
                return tag.StartsWith("speed=") || tag.StartsWith("pause=");
            }

            // send that string to textmeshpro and hide all of it, then start reading
            text = displayText;

            // lets find our custom input tag and sub it out for the correct string
            string[] inputSplit = text.Split('[', ']');
            if (inputSplit.Length > 1)
            {
                if (FindObjectOfType<OptionsHelper>().GamepadController)
                {
                    text = inputSplit[0] + ControllerInputs[inputSplit[1]] + inputSplit[2];
                }
                else
                {
                    text = inputSplit[0] + KeyboardInputs[inputSplit[1]] + inputSplit[2];
                }
            }

            maxVisibleCharacters = 0;
            readCoroutine = StartCoroutine(Read());


            IEnumerator Read()
            {
               //Debug.Log("Read Coroutine");
                int subCounter = 0;
                int visibleCounter = 0;
                while (subCounter < subTexts.Length)
                {
                    if (subCounter % 2 == 1)
                    {
                        yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                    }
                    else
                    {
                        while (visibleCounter < subTexts[subCounter].Length)
                        {
                            if (!ReadPaused)
                            {
                                onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
                                visibleCounter++;
                                maxVisibleCharacters++;
                            }
                            yield return new WaitForSecondsRealtime(1f / speed);

                        }
                        visibleCounter = 0;
                    }
                    subCounter++;
                }
                yield return null;

                WaitForSecondsRealtime EvaluateTag(string tag)
                {
                    if (tag.Length > 0)
                    {
                        if (tag.StartsWith("speed="))
                        {
                            speed = float.Parse(tag.Split('=')[1]);
                        }
                        else if (tag.StartsWith("pause="))
                        {
                            return new WaitForSecondsRealtime(float.Parse(tag.Split('=')[1]));

                        }
                    }
                    return null;
                }
                onDialogueFinish.Invoke();
            }
        }

        public void SkipText(string newText)
        {
            LoadInputDictionaries();


            text = string.Empty;
            // split the whole text into parts based off the <> tags 
            // even numbers in the array are text, odd numbers are tags
            string[] subTexts = newText.Split('<', '>');

            // textmeshpro still needs to parse its built-in tags, so we only include noncustom tags
            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                    displayText += subTexts[i];
                else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                    displayText += $"<{subTexts[i]}>";
            }
            // check to see if a tag is our own
            bool isCustomTag(string tag)
            {
                return tag.StartsWith("speed=") || tag.StartsWith("pause=");
            }

            // send that string to textmeshpro and make sure its all visible
            text = displayText;

            string[] inputSplit = text.Split('[', ']');
            if (inputSplit.Length > 1)
            {
                if (FindObjectOfType<OptionsHelper>().GamepadController)
                {
                    text = inputSplit[0] + ControllerInputs[inputSplit[1]] + inputSplit[2];
                }
                else
                {
                    text = inputSplit[0] + KeyboardInputs[inputSplit[1]] + inputSplit[2];
                }
            }
            maxVisibleCharacters = displayText.Length;
            onDialogueFinish.Invoke();
        }
    }
}