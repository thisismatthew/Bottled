using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Rigidbody rb;
    // Update is called once per frame
    void Update()
    {
        
        if (rb != null)
        {
            rb = this.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            rb.rotation.Set(0,0,0,0);
        }
        else
        {
            try
            {
                rb = GetComponent<Rigidbody>();
            }
            catch
            {
                //this is aweful code. 
            }
        }
    }
}
