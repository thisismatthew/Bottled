using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinChain : MonoBehaviour
{
    public Transform pinPos;
    // Update is called once per frame
    void Update()
    {
        transform.position = pinPos.position;
    }
}
