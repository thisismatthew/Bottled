using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class ObiRopeClimber : MonoBehaviour
{
    public ObiRope Rope;
    private int _actorParticles;
    public int _closestParticleIndex;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(Rope != null);
       
    }

    // Update is called once per frame
    void Update()
    {
        _closestParticleIndex = FindClosestRopeParticle(transform.position);
        Debug.DrawLine(Rope.GetParticlePosition(_closestParticleIndex), transform.position, Color.red);
    }

    public int FindClosestRopeParticle(Vector3 target)
    {
        //Get the full length of the rope.
        _actorParticles = Rope.solverIndices.Length;

        var closestIndexDistance = 1000f;
        int closestIndex = 0;

        //Find nearest particle to players hand
        foreach (int solverIndex in Rope.solverIndices)
        {
            Vector3 pos = Rope.GetParticlePosition(solverIndex);

            float distance = Vector3.Distance(target, pos);
            if (distance < closestIndexDistance)
            {
                closestIndexDistance = distance;
                closestIndex = solverIndex;
            }
        }
        return closestIndex;
    }
}
