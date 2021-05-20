using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cauldron : MonoBehaviour
{
    public List<Recipe> Recipes = new List<Recipe>();
    public Recipe Cauldronrecipe;
    public bool RecipeMade = false;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Starting ingredients in Cauldran
        Cauldronrecipe.Ingredients[0] = Ingredient.apple;
        Cauldronrecipe.Ingredients[1] = Ingredient.Object;
        Cauldronrecipe.Ingredients[2] = Ingredient.carrot;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CompareRecipes(List<Ingredient>IngredientSet, List<Ingredient> RecipeCheck)
    {
        IEnumerable<Ingredient> matchingIngredients = from ingredient in IngredientSet.Intersect(RecipeCheck)
        select ingredient;
        if (matchingIngredients.Count() == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        //the item must have the tag PickUpable for the collider to detect them 
        if (collider.gameObject.tag == "PickUpable")
        {
            Ingredient ingredientAdded = collider.gameObject.GetComponent<Pickupable>().IngredientName;
            Destroy(collider.gameObject);
            Cauldronrecipe.Ingredients[count] = ingredientAdded;
            count++;
            if (count == 3)
            {
                foreach (Recipe recipe in Recipes)
                {
                    RecipeMade = CompareRecipes(Cauldronrecipe.Ingredients, recipe.Ingredients);
                    if (RecipeMade)
                    {
                        //do what is needed to be done with curent recipe
                        //fill dispenser
                        RecipeMade = false;
                    }
                }
            }
        }
    }

}
