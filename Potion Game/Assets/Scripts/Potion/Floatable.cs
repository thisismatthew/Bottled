using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatable : MonoBehaviour
{
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public bool Floating = false;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        posOffset.y += amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (Floating)
            {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }

}
