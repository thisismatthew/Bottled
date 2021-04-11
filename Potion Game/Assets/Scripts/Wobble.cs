using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    SkinnedMeshRenderer rend;
    [SerializeField] Animator anim;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;
    public float MaxWobble = 0.003f;
    public float WobbleSpeed = 0.2f;
    public float Recovery = 2f;
    float wobbleAmountX;
    float wobbleAmountZ;
    float waveSize;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;

    float pulse;
    float time = 0.3f;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));
        waveSize = Mathf.Lerp(waveSize, 0, Time.deltaTime * (Recovery));
        // make a sine wave of the decreasing wobble

        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);
        waveSize = (Mathf.Abs(wobbleAmountX) + Mathf.Abs(wobbleAmountZ));

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);
        //Debug.Log("wbbb is: " + wobbleAmountX + " "+ wobbleAmountZ);

        float waveUpdate;
        waveUpdate = (waveSize > 0.1f) ? waveSize*0.7f : 0.009f*6f;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
        {
            //Debug.Log("shoopdawoop");
            waveUpdate = 0.2f;
        }
        rend.material.SetFloat("_WaveAmplitude", (waveUpdate));

        if (waveSize > 0)
        {
            rend.material.SetFloat("_WavesBool", 1);
        }
        else
        {      
            rend.material.SetFloat("_WavesBool", 0.009f);
        }


        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((0.04f * velocity.x + (angularVelocity.z * 0.1f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((0.04f *velocity.z + (angularVelocity.x * 0.1f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }



}