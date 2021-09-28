using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cauldron : MonoBehaviour
{
    public GameObject MyDistributerCollider;
    public Ladel ladel;
    private Distributer MyDistributer;
    public List<Recipe> Recipes = new List<Recipe>();
    public Recipe Cauldronrecipe;
    public bool RecipeMade = false;
    public Dictionary<PotionAttributeName, IPotionAttribute> PotionAttributeDict = new Dictionary<PotionAttributeName, IPotionAttribute>();
    
    public List<IPotionAttribute> ListOfAttributes = new List<IPotionAttribute>();
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //linking script with enum PotionAttributeName value
        PotionAttributeDict.Add(PotionAttributeName.Fire, GetComponent<FireAttribute>());
        PotionAttributeDict.Add(PotionAttributeName.HerbalTea, GetComponent<HerbalTeaAttribute>());
        PotionAttributeDict.Add(PotionAttributeName.Water, GetComponent<WaterAttribute>());
        PotionAttributeDict.Add(PotionAttributeName.AntiGravity, GetComponent<AntiGravityAttribute>());

        //linking cauldron with distributer
        MyDistributer = MyDistributerCollider.GetComponent<Distributer>();
        //Starting ingredients in Cauldran
        Cauldronrecipe.Ingredients[0] = Ingredient.tea;
        //Cauldronrecipe.Ingredients[1] = Ingredient.Object;
        //Cauldronrecipe.Ingredients[2] = Ingredient.carrot;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CompareRecipes(List<Ingredient>IngredientSet, List<Ingredient> RecipeCheck)
    {
        //system was originally designed for 3 items so looping was required, kept architecture so it can easily be reverted if scope expands.
        IEnumerable<Ingredient> matchingIngredients = from ingredient in IngredientSet.Intersect(RecipeCheck)
        select ingredient;
        if (matchingIngredients.Count() == 1)
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
            if (count == 1) 
                
            {
                foreach (Recipe recipe in Recipes)
                {
                    RecipeMade = CompareRecipes(Cauldronrecipe.Ingredients, recipe.Ingredients);
                    if (RecipeMade)
                    {
                        foreach (PotionAttributeName attribute in recipe.RecipeAttributes)
                        {
                            ListOfAttributes.Add(PotionAttributeDict[attribute]);
                        }
                        //MyDistributer.EmptyDistributor();

                        Debug.Log("atributes to add to distributor: ");
                        foreach (var a in ListOfAttributes)
                        {
                            Debug.Log(" - " + a);
                        }
                        MyDistributer.FillDistributor(ListOfAttributes);
                        Debug.Log("Fill distributor");
                        ListOfAttributes.Clear();
                        Debug.Log("Attributes cleared");

                        ladel.StartChase();
                        //do what is needed to be done with curent recipe
                        //fill dispenser
                        RecipeMade = false;
                        count = 0;
                    }
                }
            }
        }
    }

}
