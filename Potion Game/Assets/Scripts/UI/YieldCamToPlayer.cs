using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class YieldCamToPlayer : MonoBehaviour
{
    public GameObject Dialogue;
    private void Start()
    {
        GetComponent<LockPlayer>().FlipLock();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IntroCam")
        {
            Debug.Log("trig");
            //might not need to release the player for a brief period of time since dialogue should release them. 
            GetComponent<LockPlayer>().FlipLock();
            // lets reduce the current dialogue cam to 0 priority/ 
            other.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            //initiate the dialogue event placed on the player
            Dialogue.SetActive(true);
        }
    }
}
