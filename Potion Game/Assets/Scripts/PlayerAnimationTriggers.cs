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
        }
        else if (!_spillLiquid.isEmitting)
        {
            _spillLiquid.Play();

        }
    }

    public void LeftCloud()
    {
        _leftCloudVFX.Play();
    }
    public void RightCloud()
    {
        _rightCloudVFX.Play();
    }

    public void IdleBreath()
    {
        GetComponentInChildren<Mouth>().Breath();
    }
    public void Scream()
    {
        GetComponentInChildren<Mouth>().Scream();
    }
    public void RunBreath()
    {
        GetComponentInChildren<Mouth>().RunBreath();
    }
}
