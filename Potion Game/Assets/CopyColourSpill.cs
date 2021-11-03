using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyColourSpill : MonoBehaviour
{
    // Start is called before the first frame update
    private Color PlayerLiquidColor;
    private Renderer PlayerLiquid;
    private Renderer Stream;
    private Renderer Splash;
    private Renderer Pool;
    void Start()
    {
        Stream = GetComponent<Renderer>();
        Splash = Stream.transform.GetChild(0).GetComponent<Renderer>();
        Pool = Stream.transform.GetChild(1).transform.GetChild(0).GetComponentInChildren<Renderer>();
        PlayerLiquid = FindObjectOfType<SpringSystem>().gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MatchColour();
    }

    public void MatchColour()
    {
        PlayerLiquidColor = PlayerLiquid.material.GetColor("_LiquidColourx");
        Stream.material.SetColor("_Emission", PlayerLiquidColor);
        Splash.material.SetColor("_Emission", PlayerLiquidColor);
        Pool.material.SetColor("_Emission", PlayerLiquidColor);
    }
}
