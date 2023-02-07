using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientReceiver))]

public class Taco : MonoBehaviour
{

    IngredientReceiver ingredientReceiver;

    // Start is called before the first frame update
    void Start()
    {
        ingredientReceiver = this.GetComponent<IngredientReceiver>();
        ingredientReceiver.receiverDelegate += AddIngredient;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddIngredient(Toppingable ingredient) {
        ingredient.gameObject.transform.SetParent(this.transform.parent);
        PlaceIngredients();
    }

    public void PlaceIngredients() {

    }
}
