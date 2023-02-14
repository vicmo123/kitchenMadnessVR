using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientReceiver : MonoBehaviour
{
    [HideInInspector] public List<Toppingable> ingredients;
    const int MAX_INGREDIENTS = 5;

    public delegate void ReceiverDelegate(Toppingable ingredient);
    public ReceiverDelegate receiverDelegate;

    public bool ready;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new List<Toppingable>();
        ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {

        Toppingable ingredient = collision.gameObject.GetComponent<Toppingable>();
        if (ingredient == null) {
            return;
        }
        if (ingredients.Count == MAX_INGREDIENTS) {
            return;
        }
        if (HasTypeOfIngredient(ingredient.ingredientType)) {
            return;
        }
        if (!this.ready | !ingredient.ready) {
            Debug.Log("this : " + this.ready + " | ingrdient : " + ingredient.ready);
            return;
        }

        ingredient.RemoveRigidbody();
        ingredients.Add(ingredient);
        ingredient.ReceivedInIngredientReceiver();
        receiverDelegate(ingredient);
    }

    private bool HasTypeOfIngredient(Taco.Ingredients ingredientType) {
        foreach (Toppingable toppingable in ingredients) {
            if (toppingable.ingredientType == ingredientType) {
                return true;
            }
        }
        return false;
    }

    public Toppingable GetIngredientOfType (Taco.Ingredients ingredientType) {
        foreach (Toppingable toppingable in ingredients) {
            if (toppingable.ingredientType == ingredientType) {
                return toppingable;
            }
        }
        return null;
    }
}
