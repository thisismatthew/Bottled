using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningIngredientTrigger : MonoBehaviour
{
    public IngredientRitual ritual;

    private void Start()
    {
        ritual = FindObjectOfType<IngredientRitual>();
    }

    public void SummonTrigger()
    {
        ritual.SummonIngredient = true;
    }
}
