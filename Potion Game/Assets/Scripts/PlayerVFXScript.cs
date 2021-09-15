using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXScript : MonoBehaviour
{
    public VisualEffect _runCloud;
    public VisualEffect _jumpCloudR;


    public void RunCloud()
    {
        _runCloud.Play();
    }
    public void JumpCloud()
    {
        _jumpCloudR.Play();
    }
}
