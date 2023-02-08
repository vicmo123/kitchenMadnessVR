using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toppingable : MonoBehaviour
{
    [HideInInspector] public bool isInIngredientReceiver;
    public Taco.Ingredients ingredientType;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        isInIngredientReceiver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedInIngredientReceiver() {
        isInIngredientReceiver = true;
    }

    public void RemoveRigidbody() {
        Destroy(rb);
    }
}
