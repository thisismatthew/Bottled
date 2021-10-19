using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fillable : MonoBehaviour
{
    public GameObject Liquid;
    private Animator anim;

    private void Start()
    {
        Liquid.SetActive(false);
        anim = Liquid.GetComponent<Animator>();
    }
    public void Fill()
    {
        Liquid.SetActive(true);
        anim.SetBool("Full", true);
    }

    public void Empty()
    {
        Liquid.SetActive(false);
        anim.SetBool("Full", false);

    }
}
