using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSoundTrigger : MonoBehaviour
{
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void TriggerSound()
    {
        source.Play();
    }
}
