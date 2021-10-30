using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class IntroController : MonoBehaviour
{
    public GameObject IntroCutscene;

    public bool disableOpening = false;
    private void Update()
    {
        if (disableOpening)
        {
            IntroCutscene.SetActive(false);
            OnFinishCutscene();
        }
    }
    void Start()
    {
        //enable Cutscene
        IntroCutscene.SetActive(true);
        FindObjectOfType<PlayerInputHandler>().Locked = true;
        FindObjectOfType<MainCharacterController>().AnimMovementLocked = true;
    }

    public void OnFinishCutscene()
    {
        FindObjectOfType<PlayerInputHandler>().Locked = false;
        FindObjectOfType<MainCharacterController>().AnimMovementLocked = false;
    }
}
