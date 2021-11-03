using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSelect : MonoBehaviour
{
    public GameObject Icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseOver()
    {
        Debug.Log("enter hover");
        Icon.SetActive(true);
    }
    private void OnMouseExit()
    {
        Debug.Log("enter hover");
        Icon.SetActive(false);
    }

}
