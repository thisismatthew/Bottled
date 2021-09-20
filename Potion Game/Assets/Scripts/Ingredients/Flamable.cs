using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : MonoBehaviour
{
    public bool Burning;
    private GameObject fire;
    public Transform ParticleSpawnTransform;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Flamable>() != null)
        {
            if (Burning)
            {
                var firePrefab = Resources.Load("ParticleEffects/Fire");
                fire = firePrefab as GameObject;
                collider.gameObject.GetComponent<Flamable>().Burning= true;
                GameObject childobject = Instantiate(fire, ParticleSpawnTransform);

            }
        }
    }
}
