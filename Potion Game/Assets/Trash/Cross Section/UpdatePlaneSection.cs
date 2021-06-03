using UnityEngine;

[ExecuteInEditMode]
public class UpdatePlaneSection : MonoBehaviour
{

    public GameObject m_plane;
    private Renderer m_renderer;
    // Use this for initialization
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_renderer != null && m_plane != null)
        {
            m_renderer.sharedMaterial.SetVector("_PlanePosition", new Vector4(m_plane.transform.position.x, m_plane.transform.position.y, m_plane.transform.position.z));
            m_renderer.sharedMaterial.SetVector("_PlaneNormal", new Vector4(m_plane.transform.forward.x, m_plane.transform.forward.y, m_plane.transform.forward.z));
        }
    }

    const int SPRING_COUNT = 16;
    void debugvoid()
    {
        /**int id1d = Id2dTo1d(id.xy, SPRING_COUNT);

        float springHeight = springPositionsArray[id1d];
        float velocity = spring1DVelocityArray[id1d];
        float acceleration = 0;

        int minX = (int)id.x - 1;
        int minY = (int)id.y - 1;
        int maxX = (int)id.x + 1;
        int maxY = (int)id.y + 1;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // Ignore self
                if (x == (int)id.x && y == (int)id.y)
                {
                    continue;
                }
                float nHeight = 0;
                float nVel = 0;

                // Skip out of range spring IDs
                if (x < 0
                    || y < 0
                    || x > SPRING_COUNT - 1
                    || y > SPRING_COUNT - 1)
                {
                    continue;
                }
                else
                {
                    int nId = Id2dTo1d(int2(x, y), SPRING_COUNT);
                    nHeight = springPositionsArray[nId];
                    nVel = spring1DVelocityArray[nId];
                }

                float dampingForce = (nVel - velocity) * damp;
                acceleration += ((nHeight - springHeight) * stiffness)
                                + dampingForce;
            }
        }
        spring1DVelocityArray[id1d] += acceleration + timeStep;*/
    }
}
