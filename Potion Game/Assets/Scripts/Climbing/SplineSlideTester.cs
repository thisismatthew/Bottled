using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSlideTester : MonoBehaviour
{
    public float t;
    public Spline spline;


    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = spline.GetSplinePosition(t);
        t += Time.deltaTime / 10f;
        if (t > 1)
        {
            t = 0;
        }
    }
}
