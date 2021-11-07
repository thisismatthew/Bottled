using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BurnSphere : MonoBehaviour
{
    public Renderer _spiderWeb;
    public float sphereSize=1;
    //public float Spawnersize = 1;
    public ParticleSystem _spawners;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        _spiderWeb.sharedMaterial.SetVector("_SpherePosition", transform.position);
        transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
        _spiderWeb.sharedMaterial.SetFloat("_SphereRadius", transform.localScale.magnitude);
        _spiderWeb.sharedMaterial.SetFloat("FireSpread", transform.localScale.magnitude*1.1f);
        float spawnerSize = sphereSize * 0.25f;
        _spawners.transform.localScale = new Vector3(spawnerSize, spawnerSize, spawnerSize);
        int spawnrate = Mathf.RoundToInt(1.6f*sphereSize) + 25;
        _spawners.emissionRate = spawnrate;
    }
}
