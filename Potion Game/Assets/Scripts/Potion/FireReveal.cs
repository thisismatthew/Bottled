using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReveal : MonoBehaviour
{
    public Light light;

    private Renderer _hiddenObject;

    // Start is called before the first frame update
    void Start()
    {
        _hiddenObject = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p2p = light.transform.position - _hiddenObject.transform.position;
        float distance = p2p.magnitude;
        float multiplier = 0;
        if (distance<12)
        {
            multiplier = 1 - distance / 12;
        }
        _hiddenObject.sharedMaterial.SetVector("_LightPos", light.transform.position);
        _hiddenObject.sharedMaterial.SetFloat("_Strength", multiplier);
    }
}
