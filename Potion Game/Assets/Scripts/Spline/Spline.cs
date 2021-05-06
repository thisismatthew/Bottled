using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<GameObject> Vertices;
    private List<PiecewiseCubic> _pieces = new List<PiecewiseCubic>();
    private int _POINT_MULTIPLIER = 50;
    public LineRenderer renderer;

    //the pieces of information do we need to fit cubic between two points
    //this parametric equation takes in parameters for tangents and positions for two vertices
    //gives a parametric cubic equation
    //x0 first pos, xf final pos, dx0 delta first pos, dxf delta at final position
    //some magic maths i got from https://www.youtube.com/watch?v=T8pZiWQZ63g
    private class PiecewiseCubic
    {
        Vector3 a, b, c, d;
        public PiecewiseCubic(Vector3 x0, Vector3 xf, Vector3 dx0, Vector3 dxf)
        {
            a = dxf + dx0 + 2 * (x0 - xf);
            b = 3 * (xf - x0) - 2 * dx0 - dxf;
            c = dx0;
            d = x0;
        }

        public Vector3 CalculatePoint(float t)
        {
            return a * Mathf.Pow(t, 3) + b * Mathf.Pow(t, 2) + c * t + d;
        }
    }

    private void Start()
    {
        this.gameObject.AddComponent<LineRenderer>();
        renderer = GetComponent<LineRenderer>();
        renderer.SetWidth(0.5f, 0.5f);
        renderer.SetColors(Color.red, Color.red);
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        renderer.material = whiteDiffuseMat;
    }

    // Update is called once per frame
    void Update()
    {
        //might not need to clculate this often, an inspector button could be better
        _pieces.Clear();

        for (int i = 0; i < Vertices.Count - 1; i++)
        {
            _pieces.Add(new PiecewiseCubic(
                Vertices[i].transform.position,
                Vertices[i + 1].transform.position,
                Vertices[i].transform.up * Vertices[i].GetComponent<Vertex>().maginitudeForward,
                Vertices[i + 1].transform.up * Vertices[i + 1].GetComponent<Vertex>().magnitudeBack));
        }

        renderer.SetPositions(GetPoints(_pieces));
    }

    private Vector3[] GetPoints(List<PiecewiseCubic> pcs)
    {

        int numPoints = _pieces.Count * _POINT_MULTIPLIER;
        Vector3[] points = new Vector3[numPoints];
        renderer.positionCount = numPoints;
        for (int i = 0; i < _pieces.Count; i++)
        {
            for (int j = 0; j < _POINT_MULTIPLIER; j++)
            {
                points[_POINT_MULTIPLIER * i + j] = _pieces[i].CalculatePoint(j / (float)_POINT_MULTIPLIER);
            }
        }

        return points;
    }

    public Vector3 GetSplinePosition(float t)
    {
        if (_pieces.Count > 0)
        {
            float x = Mathf.Clamp01(t) * _pieces.Count;
            return (int)t < 1 ? _pieces[(int)x].CalculatePoint(x - Mathf.Floor(x)) : Vertices[Vertices.Count - 1].transform.position;
        }
        else
        {
            Debug.Log("Reached end of spline");
            return Vector3.zero;
        }
    }

    public Vector3 GetClosestSplinePosition(Vector3 pos)
    {
        
        Vector3[] points = GetPoints(_pieces);
        Vector3 closestPoint = points[0];
        float currentDistance = Vector3.Distance(pos, closestPoint);
        foreach (Vector3 p in points)
        {
            if (Vector3.Distance(pos,p)< currentDistance)
            {
                closestPoint = p;
                currentDistance = Vector3.Distance(pos, closestPoint);
            }
        }
        return closestPoint;

    }

    public float SplinePosToIndex(Vector3 pos)
    {
        Vector3[] points = GetPoints(_pieces);
        float count = 0;
        foreach (Vector3 p in points)
        {
            count++;
            if (p == pos)
            {
                return count;
            }
        }
        Debug.Log("WARNING: No Position on the Spline matches given Position");
        return 0f;
    }
}