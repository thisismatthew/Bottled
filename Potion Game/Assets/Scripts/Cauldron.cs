using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    private int count = 0;
    private Recipe recipe;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("count" + count);
        Debug.Log("count" + recipe);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider PickUpable)
    {
        //the player must have the tag Player for the collider to detect them 
        if (PickUpable.gameObject.tag == "PickUpable")
        {
            
            Debug.Log("Ingredient added!!!");
            recipe.Ingredients[count] = PickUpable.gameObject;
            Destroy(PickUpable.gameObject);
            count++;
            Debug.Log("count" + count);
            Debug.Log("count" + recipe);
        }
    }

}
