using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyColour : MonoBehaviour
{
    // Start is called before the first frame update
    private Color PlayerLiquidColor;
    public Renderer PlayerLiquid;
    private Renderer Deathball;
    private Renderer Deathpool;
    void Start()
    {
        Deathball = GetComponent<Renderer>();
        Deathpool = Deathball.transform.GetChild(0).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MatchColour();
    }

    public void MatchColour()
    {
        PlayerLiquidColor = PlayerLiquid.material.GetColor("_LiquidColourx");
        Deathball.material.SetColor("_em", PlayerLiquidColor);
        Deathpool.material.SetColor("_Emission", PlayerLiquidColor);
    }
}
