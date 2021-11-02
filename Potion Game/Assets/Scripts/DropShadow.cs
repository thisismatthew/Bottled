using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour
{
    /*
     * The wanted length for the Raycast.
     */
    public float distance = 100f;
    public MeshRenderer _dropShadow;
    public bool _climbShadowRemove = false;

    void LateUpdate()
    {
        _dropShadow.GetComponent<MeshRenderer>().enabled = !_climbShadowRemove;
        /*
         * Create the hit object.
         */
        /*
         * Cast a Raycast.
         * If it hits something:
         */
        Vector3 targetLocation = new Vector3(transform.position.x, FiveRays(), transform.position.z);

        targetLocation += new Vector3(0, _dropShadow.transform.localScale.y / 8, 0);
        /*
         * Move the object to the target location.
         */
        _dropShadow.transform.position = targetLocation;
        //Debug.Log("target is at " + _dropShadow.position);
    }

    float FiveRays()
    {
        Vector3 north = RayHit(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f));
        Vector3 south = RayHit(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f));
        Vector3 east = RayHit(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z));
        Vector3 west = RayHit(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z));
        Vector3[] cardinals = { north, south, east, west};
        float tallest = transform.position.y;
        {
            for (int i = 0;i< 4;i++)
            {
                if (cardinals[i].y < tallest-0.05f)
                {
                    //Debug.Log("testerino "+ cardinals[i]);
                    tallest = cardinals[i].y;
                }

            }
        }
        return tallest;
    }

    Vector3 RayHit(Vector3 Cardinal)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, distance);
        return hit.point;
    }
}
