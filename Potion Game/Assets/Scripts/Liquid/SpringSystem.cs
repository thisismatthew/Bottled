using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSystem : MonoBehaviour
{
    const int SPRING_COUNT = 16;

    [Range(0.1f, 0.999f)] public float Damping = 0.1f;
    [Range(0.1f, 300.0f)] public float SpringStiffness = 5.0f;

    public Transform target;
    public Transform targetRoot;

    public ComputeShader calculationEngine;
    public ParticleSystem bubbles;
    private RenderTexture texture;
    private Renderer m_renderer;
    private int physicsSimID;
    private ComputeBuffer propertiesBuffer;
    private ComputeBuffer _springHeightBuffer;
    private ComputeBuffer _spring1DVelocityBuffer;
    private ComputeBuffer deltaTimeBuffer;
    private ComputeBuffer externalForcesBuffer;


    private Vector3 lastPos;
    private Vector3 lastUp;
    private float[] extForce = new float[SPRING_COUNT * SPRING_COUNT];
    private float[] delta = new float[1];

    public RenderTexture getSimulatedTexture
    {
        get { return texture; }
    }


    void Start()
    {
        texture = new RenderTexture(16, 16, 0);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.enableRandomWrite = true;
        texture.Create();
        target.position = new Vector3(targetRoot.position.x, targetRoot.position.y, targetRoot.position.z);
        m_renderer = GetComponent<Renderer>();

        _springHeightBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));
        _spring1DVelocityBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));

        deltaTimeBuffer = new ComputeBuffer(1, sizeof(float));
        externalForcesBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));
        propertiesBuffer = new ComputeBuffer(2, sizeof(float));


        float[] velArray = new float[SPRING_COUNT*SPRING_COUNT];
        float[] posArray = new float[SPRING_COUNT*SPRING_COUNT];
        for (int y = 0; y < SPRING_COUNT; y++)
        {
            for (int x = 0; x < SPRING_COUNT; x++)
            {
                posArray[y + x * SPRING_COUNT] = 0.5f;
            }
        }
        _springHeightBuffer.SetData(posArray);
        _spring1DVelocityBuffer.SetData(velArray);
        SetGridProperties();

        lastPos = transform.position;
  

    }

    void FixedUpdate()
    {

        delta[0] = Time.fixedDeltaTime;

        //finding the liquid mesh parameters and liquid levels
        Vector3 bottleCenter = m_renderer.bounds.center;
        Vector3 pointAtVec = bottleCenter - target.position;
        Vector3 pointNormal = pointAtVec.normalized;

        //Getting angles to reorientate the liquid
        Ray ray = new Ray(bottleCenter, pointNormal);
        float waveangle = Vector3.Angle(ray.direction.normalized, transform.parent.forward);
        float sphereangle = 180 - transform.parent.rotation.eulerAngles.y;

        //sending data to shader
        m_renderer.material.SetVector("_planeNormal", pointNormal);
        m_renderer.material.SetFloat("_Rotate", sphereangle);
        m_renderer.material.SetFloat("_Rotate2", sphereangle- waveangle);

        //velocity to scale the waves
        Vector3 velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        Vector3 rotVelocity = (transform.up - lastUp) / Time.fixedDeltaTime;

        //combines the velocities into a value for scaling
        float vscale = 0.1f*Mathf.SmoothStep(0,20,velocity.magnitude + rotVelocity.magnitude);

        //sin wave to send to the compute shader
        for (int y = 0; y < SPRING_COUNT; y++)
        {
            for (int x = 0; x < SPRING_COUNT; x++)
            {
                extForce[y + x * SPRING_COUNT] = vscale * Mathf.Sin((x + 7.5f) / 4.75f);
            }
        }

        //for the veloctites
        lastPos = transform.position;
        lastUp = transform.up;


        // Spring Velocity Kernel
        //values to be sent to the compute shader
        deltaTimeBuffer.SetData(delta);
        physicsSimID = calculationEngine.FindKernel("CSMainVel");
        calculationEngine.SetBuffer(physicsSimID, "springPositionsArray", _springHeightBuffer);
        calculationEngine.SetBuffer(physicsSimID, "spring1DVelocityArray", _spring1DVelocityBuffer);
        calculationEngine.SetBuffer(physicsSimID, "propertiesBuffer", propertiesBuffer);
        calculationEngine.SetBuffer(physicsSimID, "deltaTimeBuffer", deltaTimeBuffer);
        calculationEngine.Dispatch(physicsSimID, SPRING_COUNT, SPRING_COUNT, 1);



        // Spring Position Kernel
        //values to be sent to the compute shader
        physicsSimID = calculationEngine.FindKernel("CSMainPos");
        calculationEngine.SetBuffer(physicsSimID, "springPositionsArray", _springHeightBuffer);
        calculationEngine.SetBuffer(physicsSimID, "spring1DVelocityArray", _spring1DVelocityBuffer);
        externalForcesBuffer.SetData(extForce);
        calculationEngine.SetBuffer(physicsSimID, "externalForcesBuffer", externalForcesBuffer);
        calculationEngine.SetBuffer(physicsSimID, "deltaTimeBuffer", deltaTimeBuffer);
        calculationEngine.SetTexture(physicsSimID, "Result", texture);
        calculationEngine.Dispatch(physicsSimID, SPRING_COUNT, SPRING_COUNT, 1);

        /*
        float[] test = new float[16 * 16];
        _springHeightBuffer.GetData(test);
        Debug.Log("line1 " + test[0] + "," + test[1] + "," + test[2] + "," + test[3] +","+ test[4] + "," + test[5] + "," + test[6] + "," + test[7] +","+ test[8] + "," + test[9] + "," + test[10] + "," + test[11] +","+ test[12] + "," + test[13] + "," + test[14] + "," + test[15]);
        Debug.Log("line2 " + test[14*15 + 0] + "," + test[14 * 15 + 1] + "," + test[14 * 15 + 2] + "," + test[14 * 15 + 3] +","+ test[14 * 15 + 4] + "," + test[14 * 15 + 5] + "," + test[14 * 15 + 6] + "," + test[14 * 15 + 7] +","+ test[14 * 15 + 8] + "," + test[14 * 15 + 9] + "," + test[14 * 15 + 10] + "," + test[14 * 15 + 11] + test[14 * 15 + 12] + "," + test[14 * 15 + 13] + "," + test[14 * 15 + 14] + "," + test[14 * 15 + 15]);
        */

        //sends the compute shader generated texture to the main shader
        m_renderer.sharedMaterial.SetTexture("_WaveDeformTex", texture);

    }
    private void Update()
    {
        //updates the position of the swing root, and height of the swing weight by framerate for no jerky movement o
        //loopty loops when jumping. Does not affect the phycics otherwise
        Vector3 bottleCenter = m_renderer.bounds.center;
        targetRoot.position = new Vector3(bottleCenter.x, targetRoot.position.y, bottleCenter.z);
        target.position = new Vector3(target.position.x, targetRoot.position.y, target.position.z);
    }

    private void OnDestroy()
    {
        if (_springHeightBuffer != null)
            _springHeightBuffer.Release();
        if (_spring1DVelocityBuffer != null)
            _spring1DVelocityBuffer.Release();
        if (propertiesBuffer != null)
            propertiesBuffer.Release();
        if (deltaTimeBuffer != null)
            deltaTimeBuffer.Release();
        if (externalForcesBuffer != null)
            externalForcesBuffer.Release();
    }

    void SetGridProperties()
    {
        propertiesBuffer.SetData(new float[] {Damping, SpringStiffness});
    }



}
