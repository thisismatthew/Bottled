using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool openingDone = false;
    public Sound[] sounds;
    public AudioMixerGroup MixerGroupOutput;

    // This is Brackeys Audio Manager system with some small tweaks. 
    void Awake()
    {       
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = MixerGroupOutput;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
            return true;
        return false;
    }

    public void StopAll()
    {
        foreach(Sound s in sounds)
        {
            if (IsPlaying(s.name))
            {
                Stop(s.name);
            }
        }
    }

    public void SetPitch(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = value;
    }
}
