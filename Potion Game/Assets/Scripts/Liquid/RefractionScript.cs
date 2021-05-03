using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionScript : MonoBehaviour
{
    private ReflectionProbe PotionProbe;
    private Texture probeCubeMap;
    private int frames;
    private Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        frames = 0;
        m_renderer = GetComponent<Renderer>();
        PotionProbe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames == 10)
        {
            frames = 0;
            probeCubeMap = PotionProbe.realtimeTexture;
            m_renderer.material.SetTexture("_CubeMap", probeCubeMap);
        }
    }
}
