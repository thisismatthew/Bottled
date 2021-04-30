using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollerCameraController : MonoBehaviour
{
    public Spline spline;
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = spline.GetClosestSplinePosition(target.transform.position);
    }

    
}
