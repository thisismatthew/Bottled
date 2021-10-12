using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillCollider : MonoBehaviour
{
    public List<GameObject> ObjectsInSplashZone;

    private void OnTriggerEnter(Collider other)
    {
        ObjectsInSplashZone.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        ObjectsInSplashZone.Remove(other.gameObject);
    }
}
