using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private AudioManager audio;
    private void Start()
    {
        audio = FindObjectOfType<AudioManager>();
    }
    public void TriggerDefault()
    {
        GetComponentInParent<KinematicCharacterController.Examples.MainCharacterController>().ReturnToDefaultState();
    }

    public void PlayStepSound()
    {
        audio.SetPitch("step", Random.Range(0.8f, 1.2f));
        audio.Play("step");
    }

    public void PlayJumpSound()
    {
        audio.SetPitch("jump", Random.Range(0.8f, 1.2f));
        audio.Play("jump");
    }
}
