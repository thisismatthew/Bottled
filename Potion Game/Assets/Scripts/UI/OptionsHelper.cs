using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHelper : MonoBehaviour
{
    public bool Fullscreen, GamepadController;
    public float MasterVolume;
    private float _currentVolume;
    private AudioManager _audioManager;
    private void Start()
    {
         DontDestroyOnLoad(transform.gameObject);
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (_currentVolume != MasterVolume)
        {
            _currentVolume = MasterVolume;
            foreach(Sound s in _audioManager.sounds)
            {
                s.source.volume = _currentVolume;
            }
        }
    }
}
