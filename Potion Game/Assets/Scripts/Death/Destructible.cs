using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject _ShatteredGlass;
    public float radius = 5.0F;
    public float power = 10.0F;
    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            Vector3 explosionPos = transform.position;
            Instantiate(_ShatteredGlass, explosionPos, transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
            Destroy(gameObject);
        }
    }
}
