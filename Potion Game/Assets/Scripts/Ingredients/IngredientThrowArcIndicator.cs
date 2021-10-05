using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class IngredientThrowArcIndicator : MonoBehaviour
{
    public MainCharacterController player;
    private Pickupable ingredient;
    private void Update()
    {
        if (player.NearCauldron && player.Interactable != null)
        {
            ingredient = player.Interactable.GetComponent<Pickupable>();
            Debug.Log("near cauldron with ingredient");
            ingredient.ProjectThrowTrajectory(player.CauldronThrowTarget.position);

        }
    }

}
