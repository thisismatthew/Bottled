using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Rigidbody rb;
    public bool LockMovement = false;
    // Update is called once per frame
    void Update()
    {
        
        if (rb != null)
        {
            rb = this.GetComponent<Rigidbody>();
            rb.constraints = 0;
            float y = transform.rotation.y;
            float w = transform.rotation.w;
            transform.rotation = new Quaternion(0f, y, 0f, w);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if (LockMovement)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.useGravity = false;
            }
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
