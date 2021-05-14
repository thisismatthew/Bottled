using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSystem : MonoBehaviour
{
    const int SPRING_COUNT = 16;

    [Range(0.1f, 0.999f)] public float Damping = 0.1f;
    [Range(0.1f, 100.0f)] public float SpringStiffness = 5.0f;
    [Range(0.1f, 0.999f)] public float verticalLimit = 0.2f;

    public Transform target;

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

        m_renderer = GetComponent<Renderer>();

        _springHeightBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));
        _spring1DVelocityBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));

        deltaTimeBuffer = new ComputeBuffer(1, sizeof(float));
        externalForcesBuffer = new ComputeBuffer(SPRING_COUNT * SPRING_COUNT, sizeof(float));
        propertiesBuffer = new ComputeBuffer(2, sizeof(float));


        float[] velArray = new float[SPRING_COUNT*SPRING_COUNT];
        float[] posArray = new float[SPRING_COUNT*SPRING_COUNT];

        _springHeightBuffer.SetData(posArray);
        _spring1DVelocityBuffer.SetData(velArray);
        SetGridProperties();

        lastPos = transform.position;
  

    }

    void FixedUpdate()
    {
        // Spring Velocity Kernel
        deltaTimeBuffer.SetData(delta);

        physicsSimID = calculationEngine.FindKernel("CSMainVel");
        calculationEngine.SetBuffer(physicsSimID, "springPositionsArray", _springHeightBuffer);
        calculationEngine.SetBuffer(physicsSimID, "spring1DVelocityArray", _spring1DVelocityBuffer);
        calculationEngine.SetBuffer(physicsSimID, "propertiesBuffer", propertiesBuffer);
        calculationEngine.SetBuffer(physicsSimID, "deltaTimeBuffer", deltaTimeBuffer);
        calculationEngine.Dispatch(physicsSimID, SPRING_COUNT, SPRING_COUNT, 1);



        // Spring Position Kernel
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
        Debug.Log("line1 " + test[0] + "," + test[1] + "," + test[2] + "," + test[3]);
        Debug.Log("line2 " + test[8 + 0] + "," + test[8 + 1] + "," + test[8 + 2] + "," + test[8 + 3]);
        Debug.Log("line3 " + test[16 + 0] + "," + test[16 + 1] + "," + test[16 + 2] + "," + test[16 + 3]);*/
    }
    private void Update()
    {
        delta[0] = Time.deltaTime;
        Vector3 velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        Vector3 rotVelocity = (transform.up - lastUp) / Time.fixedDeltaTime;

        lastPos = transform.position;
        lastUp = transform.up;
        Vector3 pointAtVec = transform.parent.position - target.position;
        //pointAtVec = -pointAtVec;
        //Debug.Log("pointAtVec " + pointAtVec.x + " " + pointAtVec.y + " " + pointAtVec.z);
        Vector3 hh = pointAtVec.normalized;
        //bubbles.transform.up = pointAtVec;

        //Debug.Log("hh " + hh.x + " " + hh.y + " " + hh.z);
        m_renderer.material.SetVector("_planeNormal", pointAtVec);

        
        for (int x = 0; x < SPRING_COUNT; x++)
        {
            float wobx = velocity.x+rotVelocity.z;
            float wobz = velocity.z + rotVelocity.x;
            float vscale = Mathf.Sqrt(wobx * wobx + wobz * wobz);
            float neg = -1;
 

            extForce[0 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[1 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[2 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[3 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[4 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[5 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[6 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[7 + x * SPRING_COUNT] = Mathf.Clamp(vscale, -verticalLimit, verticalLimit);
            extForce[8 + x * SPRING_COUNT] = Mathf.Clamp(neg*vscale, -verticalLimit, verticalLimit);
            extForce[9 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[10 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[11 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[12 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[13 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[14 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
            extForce[15 + x * SPRING_COUNT] = Mathf.Clamp(neg * vscale, -verticalLimit, verticalLimit);
        }

        m_renderer.sharedMaterial.SetTexture("_WaveDeformTex", texture);
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
