using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSystem : MonoBehaviour
{

    //The following code and compute shader is based on the work by Andy Green.
    //http://andytech.art/container-water-00
    //The connected shaders also use his code.

    const int SPRING_COUNT = 16;
    [Header("Waves")]
    [Range(0.1f, 5f)] public float Damping = 0.1f;
    [Range(0.1f, 300.0f)] public float SpringStiffness = 5.0f;

    [Range(0.001f, 0.1f)] public float perpMotionMagnitude = 0.015f;
    [Range(0f, 200f)] public float perpMotionSpeed = 113f;

    public Transform target;
    public Transform targetRoot;
    public Transform MainCharacterParent;

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

    private float perpetualMotionCount;
    private float[] extForce = new float[SPRING_COUNT * SPRING_COUNT];
    private float[] delta = new float[1];
    private int liquidpulse = 0;
    private float liquidwave = 0;
    private bool swishing = false;
    private Queue<Vector3> lastVelo = new Queue<Vector3>();
    private Animator anim;
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
  
        for (int i=0;i<3;i++)
        {
            lastVelo.Enqueue(Vector3.zero);
        }

        anim = MainCharacterParent.GetChild(0).GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        delta[0] = Time.fixedDeltaTime;

        //combines the velocities into a value for scaling
        Vector3 velo = MainCharacterParent.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().Velocity;
        bool dancin = anim.GetCurrentAnimatorStateInfo(0).IsName("Dance");
        float dancemulti = 1;
        if (velo.magnitude < lastVelo.Dequeue().magnitude * 0.8f && velo.magnitude > 1f && swishing == false)
        {
            swishing = true;
            liquidwave = 0;
            liquidpulse = 0;
        }

        if ( dancin== true && swishing == false)
        {
            swishing = true;
            liquidwave = 0;
            liquidpulse = 0;
        }

        float oldmulti = m_renderer.sharedMaterial.GetFloat("_SpringMultiplier");
        if (dancin)
        {
            dancemulti = 2;          
            m_renderer.sharedMaterial.SetFloat("_SpringMultiplier", Mathf.Lerp(oldmulti,0.4f,0.05f));
        }
        else
        {
            m_renderer.sharedMaterial.SetFloat("_RunMultiplier", velo.magnitude / 5);
            m_renderer.sharedMaterial.SetFloat("_Height", Mathf.Clamp(-0.5f - velo.magnitude / 15, -0.85f, -0.5f));
            m_renderer.sharedMaterial.SetFloat("_SpringMultiplier", Mathf.Lerp(oldmulti, 0.25f, 0.05f));
        }
        lastVelo.Enqueue(velo);

        //Debug.Log(velo.magnitude);
        //sin wave to send to the compute shader
        //Debug.Log("The velo" + velo);

        if (swishing == true)
        {
            if (liquidpulse < 40)
            {
                /*for (int y = 0; y < SPRING_COUNT; y++)
                {
                    for (int x = 0; x < SPRING_COUNT - 2; x++)
                    {
                        extForce[2 + x * SPRING_COUNT] = liquidwave;
                        extForce[13 + x * SPRING_COUNT] = -liquidwave;
                    }
                }*/

                for (int y = 2; y < SPRING_COUNT-2; y++)
                {
                    for (int x = 2; x < SPRING_COUNT - 2; x++)
                    {
                        extForce[2 + x * SPRING_COUNT] = -liquidwave* dancemulti;
                        extForce[14 + x * SPRING_COUNT] = liquidwave* dancemulti;
                    }
                }

                if (liquidpulse <20)
                    liquidwave += 1;
                else
                    liquidwave -= 1f;
            }
            else
            {
                swishing = false;
            }

            //Debug.Log("liquid " + liquidwave);
            //Debug.Log("timer " + liquidpulse);
            liquidpulse++;
        }
        else
        {
            for (int y = 0; y < SPRING_COUNT; y++)
            {
                for (int x = 0; x < SPRING_COUNT - 2; x++)
                {
                    extForce[2 + x * SPRING_COUNT] = 0;
                    extForce[13 + x * SPRING_COUNT] = 0;
                }
            }
        }



        //Debug.Log("it is " + liquidpulse);

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
        Vector3 bottleCenter = m_renderer.bounds.center;
        if (perpetualMotionCount > 2 * perpMotionSpeed)
        {
            perpetualMotionCount = 1;
        }
        float spinModX = Mathf.Sin(perpetualMotionCount * Mathf.PI / perpMotionSpeed);
        float spinModZ = Mathf.Cos(perpetualMotionCount * Mathf.PI / perpMotionSpeed);
        perpetualMotionCount += 1;

        targetRoot.position = new Vector3(bottleCenter.x + spinModX * perpMotionMagnitude, targetRoot.position.y, bottleCenter.z + spinModZ * perpMotionMagnitude);

        target.position = new Vector3(target.position.x, targetRoot.position.y, target.position.z);
        //finding the liquid mesh parameters and liquid levels
        Vector3 pointAtVec = bottleCenter - target.position;
        //Debug.Log(pointAtVec);
        //pointAtVec = new Vector3(0.6f*pointAtVec.x, pointAtVec.y, 0.1f * pointAtVec.z);
        Vector3 pointNormal = NormalizePrecise(pointAtVec);

        //Getting angles to reorientate the liquid
        Ray ray = new Ray(bottleCenter, pointNormal);
        float waveangle = Vector3.Angle(ray.direction.normalized, MainCharacterParent.forward);
        float sphereangle = 180 - MainCharacterParent.rotation.eulerAngles.y;

        //sending data to shader
        m_renderer.material.SetVector("_planeNormal", pointNormal);
        m_renderer.material.SetFloat("_Rotate", sphereangle);
        m_renderer.material.SetFloat("_Rotate2", sphereangle - waveangle);
    }
    private void Update()
    {
        //updates the position of the swing root, and height of the swing weight by framerate for no jerky movement or
        //loopty loops when jumping. Does not affect the physics otherwise

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

    Vector3 NormalizePrecise(Vector3 v)
    {
        float mag = v.magnitude;

        if (mag == 0) return Vector3.zero;

        return (v / mag);

    }

}
