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
        Debug.Log(this.gameObject.name + "Intro Controller");
        //enable Cutscene
        IntroCutscene.SetActive(true);
        LockPlayer.Instance.FlipLock();
    }

    public void OnFinishCutscene()
    {
        LockPlayer.Instance.FlipLock();
    }
}
