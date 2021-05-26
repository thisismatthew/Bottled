using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName ="Recipe")]
public class Recipe : ScriptableObject
{
    public List<Ingredient> Ingredients = new List<Ingredient>(3);
    public List<IPotionAttribute> RecipeAttributes = new List<IPotionAttribute>();
    //public variable for attributes assoicated with recipe

}
