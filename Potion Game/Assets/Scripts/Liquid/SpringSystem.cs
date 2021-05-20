using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSystem : MonoBehaviour
{
    const int SPRING_COUNT = 16;

    [Range(0.1f, 0.999f)] public float Damping = 0.1f;
    [Range(0.1f, 100.0f)] public float SpringStiffness = 5.0f;

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

        _springHeightBuffer.SetData(posArray);
        _spring1DVelocityBuffer.SetData(velArray);
        SetGridProperties();

        lastPos = transform.position;
  

    }

    void FixedUpdate()
    {

        delta[0] = Time.fixedDeltaTime;

        Vector3 bottleCenter = new Vector3(transform.position.x, transform.position.y + 0.385f, transform.position.z);
        Vector3 bottleTop = new Vector3(transform.position.x, transform.position.y + 0.3f + 2 * 0.385f, transform.position.z);
        target.position = new Vector3(target.position.x, targetRoot.position.y, target.position.z);
        Vector3 pointAtVec = bottleCenter - target.position;
        Vector3 pointNormal = pointAtVec.normalized;
        Vector3 relativeSpherePosition = bottleTop - target.position;
        //bubbles.transform.up = pointAtVec;

        m_renderer.material.SetVector("_planeNormal", pointNormal);
        //Debug.Log(pointNormal);
        m_renderer.material.SetVector("_SpherePosition", relativeSpherePosition);

        Vector2 externalVector = new Vector2(pointAtVec.x, pointAtVec.z).normalized;
        Debug.Log(externalVector);

        Vector3 velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        Vector3 rotVelocity = (transform.up - lastUp) / Time.fixedDeltaTime;

        float vscale = velocity.magnitude;
        float neg1 = 1;
        if (externalVector.x<0)
        {
            neg1 = -1;
        }
        float neg2 = -neg1;
        for (int x = 0; x < SPRING_COUNT; x++)
        {
            for (int y = 0; y < SPRING_COUNT; y++)
            {
                if (y < SPRING_COUNT / 2)
                {
                    extForce[x + y * SPRING_COUNT] = vscale*neg1 * Mathf.Cos(x*Mathf.PI/15);
                }
                else
                {
                    extForce[x + y * SPRING_COUNT] = vscale*neg2 * Mathf.Cos(x * Mathf.PI / 15);
                }
            }    
        }
        if (vscale<0.4)
        {
            for (int x = 0; x < SPRING_COUNT; x++)
            {
                for (int y = 0; y < SPRING_COUNT; y++)
                {
                    extForce[x + y * SPRING_COUNT] = -1f;
                }
            }
        }
        m_renderer.sharedMaterial.SetTexture("_WaveDeformTex", texture);


        lastPos = transform.position;
        lastUp = transform.up;

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
        
        float[] test = new float[16 * 16];
        _springHeightBuffer.GetData(test);
        Debug.Log("line1 " + test[0] + "," + test[1] + "," + test[2] + "," + test[3] +","+ test[4] + "," + test[5] + "," + test[6] + "," + test[7] +","+ test[8] + "," + test[9] + "," + test[10] + "," + test[11] +","+ test[12] + "," + test[13] + "," + test[14] + "," + test[15]);
        Debug.Log("line2 " + test[8 + 0] + "," + test[8 + 1] + "," + test[8 + 2] + "," + test[8 + 3] + ","+test[8 + 4] + "," + test[8 + 5] + "," + test[8 + 6] + "," + test[8 + 7] +","+ test[8 + 8] + "," + test[8 + 9] + "," + test[8 + 10] + "," + test[8 + 11] +","+ test[8 + 12] + "," + test[8 + 13] + "," + test[8 + 14] + "," + test[8 + 15]);
        Debug.Log("line3 " + test[16 + 0] + "," + test[16 + 1] + "," + test[16 + 2] + "," + test[16 + 3] +","+ test[16 + 4] + "," + test[16 + 5] + "," + test[16 + 6] + "," + test[16 + 7] +","+ test[16 + 8] + "," + test[16 + 9] + "," + test[16 + 10] + "," + test[16 + 11] + test[16 + 12] + "," + test[16 + 13] + "," + test[16 + 14] + "," + test[16 + 15]);
        
    }
    private void Update()
<<<<<<< HEAD
    {
        delta[0] = Time.deltaTime;

        Vector3 velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        Vector3 rotVelocity = (transform.up - lastUp) / Time.fixedDeltaTime;

        lastPos = transform.position;
        lastUp = transform.up;
        Vector3 pointAtVec = transform.position - target.position;
        pointAtVec = -pointAtVec;
        m_renderer.material.SetVector("_planeNormal", pointAtVec);

        
        for (int x = 0; x < SPRING_COUNT; x++)
        {
            float anglemod = Mathf.Sign((7 * SPRING_COUNT- x * SPRING_COUNT ));
            if (anglemod == 0)
            {
                anglemod = 1;
            }
            float wobx = velocity.x+rotVelocity.z;
            float wobz = velocity.z + rotVelocity.x;
            float vscale = Mathf.Sqrt(wobx * wobx + wobz * wobz);
            float neg = -1;
            if (wobx+wobz<1)
            {
                vscale = -0.1f;
                neg = 1;
            }
            //turned debug off while I needed the console
            //Debug.Log("veloc " + wobx+ " " + wobz);
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

        m_renderer.sharedMaterial.SetTexture("_TextureSample2", texture);
=======
    {   
>>>>>>> WigglyArms
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
