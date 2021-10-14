using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillCollider : MonoBehaviour
{
    public List<GameObject> ObjectsInSplashZone;
    private List<GameObject> _validateList;
    private void Start()
    {
        ObjectsInSplashZone = new List<GameObject>();
        _validateList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectsInSplashZone.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        ObjectsInSplashZone.Remove(other.gameObject);
    }
    private void Update()
    {
        foreach (GameObject g in ObjectsInSplashZone)
        {
            if (g == null)
            {
                _validateList.Add(g);
            }
        }
        foreach (GameObject g in _validateList)
        {
            ObjectsInSplashZone.Remove(g);
        }
    }
}
