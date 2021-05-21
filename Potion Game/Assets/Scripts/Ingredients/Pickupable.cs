using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class Pickupable : MonoBehaviour
{
    public bool Flamable;
    public bool Burning;
    public Ingredient IngredientName;
    //just let the player know that this object can be picked up.
    //this will need refactoring if there are multiple interactables in range of the player. 

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            player.GetComponent<MainCharacterController>().Interactable = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            player.GetComponent<MainCharacterController>().Interactable = null;
        }
    }
}
