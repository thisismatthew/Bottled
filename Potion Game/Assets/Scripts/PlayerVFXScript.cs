using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXScript : MonoBehaviour
{
    public VisualEffect _runCloudL;
    public VisualEffect _runCloudR;


    public void RunCloudL()
    {
        _runCloudL.Play();
    }
    public void RunCloudR()
    {
        _runCloudR.Play();
    }
}
