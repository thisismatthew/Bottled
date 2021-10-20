using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public VisualEffect _leftCloudVFX;
    public VisualEffect _rightCloudVFX;
    public ParticleSystem _spillLiquid;
    public Transform _liquidSphere;
    private AudioManager audio;

    public void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        _leftCloudVFX.playRate = 2;
        _rightCloudVFX.playRate = 2;
        _spillLiquid.enableEmission = true;
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

    public void ToggleLiquidSpill()
    {
        if (_spillLiquid.isEmitting)
        {
            _spillLiquid.Stop();
            Debug.Log("spill");
        }
        else if (!_spillLiquid.isEmitting)
        {
            _spillLiquid.Play();
            Debug.Log("spill");

        }
    }

    public void BendForward()
    {
        GetComponentInParent<KinematicCharacterController.Examples.MainCharacterController>().tiltmod = 1;
    }

    public void BendBackward()
    {
        GetComponentInParent<KinematicCharacterController.Examples.MainCharacterController>().tiltmod = 0;
    }

    public void LeftCloud()
    {
        _leftCloudVFX.Play();
    }
    public void RightCloud()
    {
        _rightCloudVFX.Play();
    }
}