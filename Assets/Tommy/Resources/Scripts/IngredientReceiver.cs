using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientReceiver : MonoBehaviour
{
    public List<Toppingable> ingredients;
    const int MAX_INGREDIENTS = 5;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new List<Toppingable>();
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
        if (ingredient.isInIngredientReceiver) {
            return;
        }

        ingredients.Add(ingredient);
        ingredient.ReceivedInIngredientReceiver();
    }
}
