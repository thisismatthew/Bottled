using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spline))]
public class Ledge : MonoBehaviour
{
    private Spline _spline;
    private Transform _player;
    public float LedgeGrabDistance = 0.5f;
    private Vector3 _closestPos = new Vector3();
    void Start()
    {
        _spline = GetComponent<Spline>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //not sure if this will be too taxing on the cpu when there are a bunch of ledges around...
        _closestPos  = _spline.GetClosestSplinePosition(_player.position);
        
        if (Vector3.Distance(_closestPos, _player.position)< LedgeGrabDistance)
        {
            _player.position = _closestPos;


        }
    }
}
