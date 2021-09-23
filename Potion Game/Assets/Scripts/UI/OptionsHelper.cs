using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHelper : MonoBehaviour
{
    public bool Fullscreen, GamepadController;
    public float MasterVolume;
    private void Start()
    {
         DontDestroyOnLoad(transform.gameObject);
    }


}
