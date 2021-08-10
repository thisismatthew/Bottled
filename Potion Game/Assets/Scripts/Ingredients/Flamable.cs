using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : MonoBehaviour
{
    public bool Burning;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Flamable>() != null)
        {
            if (Burning)
            {
                collider.gameObject.GetComponent<Flamable>().Burning= true;
            }
        }
    }
}
