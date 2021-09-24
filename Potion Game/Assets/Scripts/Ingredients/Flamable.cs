using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : MonoBehaviour
{
    public bool Burning;
    public bool DestroyOnBurn = false;
    public GameObject FirePrefab;
    public Transform FlameParticlePosition;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Flamable>() != null)
        {
            if (Burning)
            {
                var flamableObject = collider.gameObject;
                if (flamableObject.GetComponent<Flamable>().Burning == false)
                {
                    flamableObject.GetComponent<Flamable>().Burning = true;
                    flamableObject.GetComponent<Flamable>().LightFire();
                }
            }
        }
    }
    public void LightFire()
    {
        if (DestroyOnBurn == false)
        {
           GameObject childobject = Instantiate(FirePrefab, FlameParticlePosition);//= Instantiate(FirePrefab) as GameObject;
           childobject.transform.localPosition = Vector3.zero;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }
}
