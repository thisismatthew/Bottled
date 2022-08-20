using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class Cobweb : MonoBehaviour
{
    public bool burnTrigger = false, animTriggered = false;
    public GameObject CobwebBurnCutscene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Flamable>() != null)
        {
            if (other.GetComponent<Flamable>().Burning)
            {
                burnTrigger = true;
            }
        }
    }

    private void Update()
    {
        if (burnTrigger && !animTriggered)
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Burn");
            GetComponent<Animator>().Play("Spiderweb Animation");
            CobwebBurnCutscene.SetActive(true);
            LockPlayer.Instance.FlipLock();
        }
    }

    public void DisableCobweb()
    {
        LockPlayer.Instance.FlipLock();
        this.gameObject.SetActive(false);
    }
}
