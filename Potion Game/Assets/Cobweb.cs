using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    private bool burnTrigger = false, animTriggered = false;
    
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
            GetComponent<Animator>().Play("Spiderweb Animation");
        }
    }

    public void DisableCobweb()
    {
        this.gameObject.SetActive(false);
    }
}
