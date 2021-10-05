using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReveal : MonoBehaviour
{
    public Light light;
    public int DistanceMod = 12;

    private Renderer _hiddenObject;

    // Start is called before the first frame update
    void Start()
    {
        _hiddenObject = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (light.enabled)
        {
            Vector3 p2p = light.transform.position - _hiddenObject.transform.position;
            float distance = p2p.magnitude;
            float multiplier = 0;
            if (distance < DistanceMod)
            {
                multiplier = 1 - distance / DistanceMod;
            }
            _hiddenObject.sharedMaterial.SetVector("_LightPos", light.transform.position);
            _hiddenObject.sharedMaterial.SetFloat("_Strength", multiplier);
        }
        else
        {
            _hiddenObject.sharedMaterial.SetFloat("_Strength", 0);
        }
    }
}
