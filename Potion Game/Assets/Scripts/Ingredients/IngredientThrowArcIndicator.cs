using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class IngredientThrowArcIndicator : MonoBehaviour
{
    public int MillisecondsSimulated = 50;
    public LineRenderer lineRenderer;
    public MainCharacterController player;
    private Pickupable ingredient;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (player.NearCauldron && player.Interactable != null)
        {
            ingredient = player.Interactable.GetComponent<Pickupable>();
            Debug.Log("near cauldron with ingredient");
            ProjectThrowTrajectory(player.CauldronThrowTarget.position);

        }
    }

    public void ProjectThrowTrajectory(Vector3 target)
    {
        lineRenderer.enabled = true;
        Vector3[] segments = CalculateArcPoints(target);

        Color startColor = Color.magenta;
        Color endColor = Color.magenta;
        startColor.a = 1f;
        endColor.a = 1f;

        lineRenderer.transform.position = segments[0] + ingredient.gameObject.transform.position;

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        lineRenderer.positionCount = MillisecondsSimulated;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, segments[i] + ingredient.gameObject.transform.position);
        }

    }

    //some classic projectile maths getting position of each displacement (pos x,y,z) by time t
    public Vector3[] CalculateArcPoints(Vector3 target)
    {
        Vector3 vel = ingredient.CalculateLaunchVelocity(target);
        float ypos, xpos, zpos, timeInterval;
        Vector3[] points = new Vector3[MillisecondsSimulated];
        for (int t = 1; t < MillisecondsSimulated + 1; t += 1)
        {
            timeInterval = (float)t;
            timeInterval = timeInterval / 10;
            ypos = vel.y * timeInterval + (0.5f * Physics.gravity.y * Mathf.Pow(timeInterval, 2));
            xpos = vel.x * timeInterval;
            zpos = vel.z * timeInterval;
            points[t - 1] = new Vector3(xpos, ypos, zpos);
        }

        return points;

    }
}
